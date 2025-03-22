using AutoMapper;
using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Models.DTOs.Token;
using CarRental.Models.DTOs.User;
using CarRental.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarRental.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserService(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<bool>> RegisterAsync(UserRegisterDTO dto)
        {
            var response = new ServiceResponse<bool>();
            if (UserExists(dto.Email, dto.Mobile))
            {
                response.Success = false;
                response.Data = false;
                response.Message = "User Already Exists";
                response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else if (dto.Password != dto.RepeatPassword)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Password doesn't Match";
                response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var user = new User()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Mobile = dto.Mobile,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();


                var verificationCode = new UserVerificationCode()
                {
                    User = user,
                    CodeHash = GenerateVerificationCodeAndSendEmail(user)
                };


                await _context.VerificationCodes.AddAsync(verificationCode);
                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "User Created Successfully";
            }

            return response;
        }
        public async Task<ServiceResponse<TokenDTO>> LoginAsync(UserLoginDTO dto)
        {
            var response = new ServiceResponse<TokenDTO>();
            var user = await _context.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User doesn't exists";
                response.StatusCode = StatusCodes.Status404NotFound;
            }
            else if (VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            {
                var tokens = GenerateTokens(user, dto.StayLoggedIn);

                response.Data = tokens;
            }

            return response;
        }
        public async Task<ServiceResponse<bool>> VerifyAccountAsync(string telNum, string code)
        {
            var response = new ServiceResponse<bool>();
            var userCode = await _context.VerificationCodes
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Mobile == telNum);

            if (userCode != null && userCode.User != null)
            {
                if (CheckVerificationCode(userCode.CodeHash, userCode.User.PasswordSalt, code))
                {
                    userCode.User.IsVerified = true;
                    response.Success = true;


                    _context.Users.Update(userCode.User);
                    await _context.SaveChangesAsync();

                    response.StatusCode = StatusCodes.Status200OK;
                }
                else
                {
                    response.Success= false;
                    response.Message = "Invalid Code";
                    response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Not found!";
                response.StatusCode = StatusCodes.Status404NotFound;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> ResendVerificationCode(int userId)
        {
            var response = new ServiceResponse<bool>();

            var userCode = await _context.VerificationCodes
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Id == userId);
            
            if(userCode == null || userCode.User == null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Not Found!";
                response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else if (userCode.User.IsVerified)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "account already verified";
                response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                var code = GenerateVerificationCodeAndSendEmail(userCode.User);

                userCode.CodeHash = code;

                _context.VerificationCodes.Update(userCode);

                await _context.SaveChangesAsync();

                response.Success = true;
                response.Data = true;
                response.Message = "Code has been reset";
                response.StatusCode = StatusCodes.Status200OK;

            }
            return response;
        }

        #region Private Methods
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        private bool UserExists(string email, string phonenumber)
        {
            return _context.Users.Any(x => x.Email == email || x.Mobile == phonenumber);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private TokenDTO GenerateTokens(User user, bool staySignedIn)
        {
            string refreshToken = string.Empty;

            if (staySignedIn)
            {
                refreshToken = GenerateRefreshToken(user);
                user.RefreshToken = refreshToken;
                user.RefreshExpirationDate = DateTime.Now.AddDays(2);
            }

            var accessToken = GenerateAccessToken(user);

            return new TokenDTO() { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        private string GenerateAccessToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var userRoleNames = user.Roles.Select(x => x.Name);

            claims.AddRange(userRoleNames.Select(role => new Claim(ClaimTypes.Role, role)));

            if (user.IsVerified)
                claims.Add(new Claim(ClaimTypes.Role, "verified"));

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWTOptions:Secret").Value));

            SigningCredentials creadentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creadentials,
                Issuer = _configuration.GetSection("JWTOptions:Issuer").Value,
                Audience = _configuration.GetSection("JWTOptions:Audience").Value
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            SecurityToken token = handler.CreateToken(securityTokenDescriptor);

            return handler.WriteToken(token);
        }

        private string GenerateRefreshToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWTOptions:Secret").Value));

            SigningCredentials creadentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = creadentials,
                Issuer = _configuration.GetSection("JWTOptions:JwtOptions:Issuer").Value,
                Audience = _configuration.GetSection("JWTOptions:JwtOptions:Audience").Value
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            SecurityToken token = handler.CreateToken(securityTokenDescriptor);

            return handler.WriteToken(token);
        }
        private byte[] GenerateVerificationCodeAndSendEmail(User user)
        {
            Random random = new Random();
            var randCode = random.Next(100000, 1000000);


            try
            {
                HelperServices.SendEmail(user.FirstName, user.Email, randCode.ToString());
            }
            catch (Exception ex)
            {
                HelperServices.WriteExceptionToFile(ex, "Email Sending At User Registration");
            }

            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(randCode.ToString()));
            }
        }
        private bool CheckVerificationCode(byte[] hash, byte[] salt, string code)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(code));
                return computedHash.SequenceEqual(hash);
            }
        }
        #endregion
    }
}
