using AutoMapper;
using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Models.DTOs.Role;
using CarRental.Models.DTOs.RoleUser;
using CarRental.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Services
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public RoleService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetRoleDTO>>> GetAllRolesAsync()
        {
            var response = new ServiceResponse<List<GetRoleDTO>>();

            var roles = await _context.Roles.ToListAsync();

            var roleDto = _mapper.Map<List<GetRoleDTO>>(roles);

            response.Success = true;
            response.Data = roleDto;
            response.Message = "All Roles";
            response.StatusCode = StatusCodes.Status200OK;

            return response;
        }
        public async Task<ServiceResponse<bool>> CreateRoleAsync(CreateRoleDTO dto)
        {
            var response = new ServiceResponse<bool>();

            if(await _context.Roles.AnyAsync(x => x.NormalizedName == dto.Name.ToUpper()))
            {
                response.Success = false;
                response.Data = false;
                response.Message = "This role already exists";
                response.StatusCode = StatusCodes.Status400BadRequest;

                return response;
            }

            Role role = new Role() 
            {
                Name = dto.Name,
                NormalizedName = dto.Name.ToUpper(),
            };

            await _context.Roles.AddAsync(role);

            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = true;
            response.Message = $"Role {role.Name} Added";
            response.StatusCode= StatusCodes.Status200OK;

            return response;
        }

        public async Task<ServiceResponse<bool>> UpdateRoleAsync(UpdateRoleDTO dto)
        {
            var response = new ServiceResponse<bool>();

            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if(role == null)
            {
                response.Success= false;
                response.Data = false;
                response.Message = "Role Not Found!";
                response.StatusCode = StatusCodes.Status404NotFound;
                
                return response;
            }
            if (role.NormalizedName == dto.Name.ToUpper())
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Can't rename with the same name!";
                response.StatusCode = StatusCodes.Status400BadRequest;

                return response;
            }

            role.Name = dto.Name;
            role.NormalizedName = dto.Name.ToUpper();

            _context.Roles.Update(role);

            await _context.SaveChangesAsync();

            response.Success &= response.Data == true;
            response.Message = "Role Updated";
            response.StatusCode = StatusCodes.Status200OK;

            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteRoleAsync(int Id)
        {
            var response = new ServiceResponse<bool>();

            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == Id);

            if (role == null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Role Not Found!";
                response.StatusCode = StatusCodes.Status404NotFound;

                return response;
            }

            _context.Roles.Remove(role);

            await _context.SaveChangesAsync();

            response.Success &= response.Data == true;
            response.Message = "Role Deleted";
            response.StatusCode = StatusCodes.Status200OK;

            return response;
        }

        public async Task<ServiceResponse<bool>> AddRoleToUserAsync(AddRoleToUserDTO dto)
        {
            var response = new ServiceResponse<bool>();

            var user = await _context.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == dto.UserId);

            if (user == null)
            {
                response.Success &= response.Data = false;
                response.Message = "User Doesn't Exist";
                response.StatusCode= StatusCodes.Status404NotFound;

                return response;
            }
            if (user.Roles != null && user.Roles.Any(x => x.Id == dto.RoleId))
            {
                response.Success &= response.Data = false;
                response.Message = "User Already Has This Role";
                response.StatusCode = StatusCodes.Status404NotFound;

                return response;
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(x => x.Id == dto.RoleId);

            if (role == null)
            {
                response.Success &= response.Data = false;
                response.Message = "Role Not Found!";
                response.StatusCode = StatusCodes.Status400BadRequest;

                return response;
            }

            if (user.Roles == null)
            {
                user.Roles = new List<Role>();
            }

            user.Roles.Add(role);

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            response.Success &= response.Data = true;
            response.Message = $"Role \"{role.Name}\" Added To {user.FirstName} {user.LastName}";
            response.StatusCode = StatusCodes.Status200OK;

            return response;
        }
        public async Task<ServiceResponse<bool>> RemoveRoleFromUserAsync(RemoveRoleFromUserDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
