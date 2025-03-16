using AutoMapper;
using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Models.DTOs.Token;
using CarRental.Models.DTOs.User;
using CarRental.Models.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            try
            {
                Results.Ok(response);
                if (UserExists(dto.Email, dto.Mobile))
                {
                    response.Success = false;
                    response.Data = false;
                    response.Message = "User Already Exists";
                }
                else if (dto.Password != dto.RepeatPassword)
                    throw new BadHttpRequestException("Password doesn't match");

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

                response.Success = true;
                response.Data = true;
                response.Message = "User Created Successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Data = false;
                response.Message = ex.GetFullException();
            }
            return response;
        }

        public async Task<ServiceResponse<TokenDTO>> LoginAsync(UserLoginDTO dto)
        {
            //var response = new ServiceResponse<TokenDTO>();
            //try
            //{
            //    var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            //    if (user == null)
            //        throw new BadHttpRequestException("User not found");

            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            
            //if (VerifyPasswordHash(dto.Password, user.PasswordSalt, user.PasswordHash))
            //{

            //}
            throw new NotImplementedException();
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
        #endregion
    }
}
