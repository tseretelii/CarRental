using CarRental.Models;
using CarRental.Models.DTOs.Rental;

namespace CarRental.Interfaces
{
    public interface IRentalService
    {
        public Task<ServiceResponse<bool>> RentCarAsync(int userId, RentCarDTO dto);
    }
}
