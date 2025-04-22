using CarRental.Models;

namespace CarRental.Interfaces
{
    public interface IFavoriteCarService
    {
        Task<ServiceResponse<bool>> AddCarToFavoritesAsync(int userId, int carId);
        Task<ServiceResponse<bool>> RemoveCarFromFavoritesAsync(int userId, int carId);
    }
}
