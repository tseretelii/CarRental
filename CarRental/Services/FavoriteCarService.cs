using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Services
{
    public class FavoriteCarService : IFavoriteCarService
    {
        private readonly ApplicationDbContext _context;

        public FavoriteCarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<bool>> AddCarToFavoritesAsync(int userId, int carId)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == carId);

            if (user is null || car is null)
            {

                response.Success = false;
                response.Data = false;
                response.Message = "Not Found!";
                response.StatusCode = StatusCodes.Status404NotFound;

                return response;
            }

            var userFavoritedCars = await _context.FavoriteCars
                .Where(x => x.User == user && x.Car != null && x.IsRemoved == false)
                .ToListAsync();

            if (userFavoritedCars.Select(x => x.Car.Id).Contains(carId))
            {
                response.Success = false;
                response.Data = false;
                response.Message = "car already favorited";
                response.StatusCode = StatusCodes.Status400BadRequest;

                return response;
            }

            FavoriteCar favoriteCar = new FavoriteCar()
            {
                User = user,
                Car = car,
            };

            await _context.FavoriteCars.AddAsync(favoriteCar);

            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = true;
            response.Message = $"{favoriteCar.Car.Model} Added to favorites";
            
            return response;

        }

        public async Task<ServiceResponse<bool>> RemoveCarFromFavoritesAsync(int userId, int carId)
        {
            var response = new ServiceResponse<bool>();

            var favoriteCar = await _context.FavoriteCars.FirstOrDefaultAsync(x => x.User.Id == userId && x.Car.Id == carId && x.IsRemoved == false);

            if (favoriteCar == null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "User not found";
                response.StatusCode= StatusCodes.Status400BadRequest;

                return response;
            }

            favoriteCar.IsRemoved = true;

            _context.FavoriteCars.Update(favoriteCar);

            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = true;
            response.Message = "Car removed from favorites";

            return response;

        }
    }
}
