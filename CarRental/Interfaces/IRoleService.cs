using CarRental.Models.DTOs.Role;
using CarRental.Models;
using CarRental.Models.DTOs.RoleUser;

namespace CarRental.Interfaces
{
    public interface IRoleService
    {
        public Task<ServiceResponse<List<GetRoleDTO>>> GetAllRolesAsync();
        public Task<ServiceResponse<bool>> CreateRoleAsync(CreateRoleDTO dto);
        public Task<ServiceResponse<bool>> UpdateRoleAsync(UpdateRoleDTO dto);
        public Task<ServiceResponse<bool>> DeleteRoleAsync(int Id);
        public Task<ServiceResponse<bool>> AddRolesToUserAsync(AddRoleToUserDTO dto);
        public Task<ServiceResponse<bool>> RemoveRolesFromUserAsync(RemoveRoleFromUserDTO dto);
        public Task<ServiceResponse<List<GetRoleDTO>>> GetUserRolesAsync(int userId);
    }
}
