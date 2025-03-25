using CarRental.Models;
using CarRental.Models.DTOs.Rental;
using CarRental.Models.DTOs.User;

namespace CarRental.Interfaces
{
    public interface IAdminService
    {
        public Task<ServiceResponse<List<UserGetDTO>>> GetUsersAsync();
        public Task<ServiceResponse<bool>> VerifyUserAsync(int userId);
        public Task<ServiceResponse<GetActiveRentalsResponseDTO>> GetActiveRentalsAsync();
    }
}
