using CarRental.Models;
using CarRental.Models.DTOs.Car;

namespace CarRental.Interfaces
{
    public interface ICarService
    {
        public Task<ServiceResponse<bool>> RegisterCarAsync(RegisterCarDTO dto, int userId);
        public Task<ServiceResponse<List<GetFilteredCarDTO>>> GetFilteredCarsAsync(CarFilterDTO dto);
        public Task<ServiceResponse<GetCarDTO>> GetCarAsync(int id);
        public Task<ServiceResponse<List<GetCarDTO>>> GetPaginatedCarsAsync(int page, int numberOfCars);
        public Task<ServiceResponse<bool>> UpdateCarAsync(UpdateCarDTO dto, int userId);
    }
}
