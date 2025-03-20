using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Models.DTOs.Car;
using CarRental.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<bool>> RegisterCarAsync(RegisterCarDTO dto, int userId)
        {
            var response = new ServiceResponse<bool>();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "User not found!";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;
            }

            Car car = new Car()
            {
                Brand = dto.Brand,
                Model = dto.Model,
                ReleaseDate = dto.ReleaseDate,
                Images = new List<CarImages>(),
                Price = dto.Price,
                EngineCapacity = dto.EngineCapacity,
                Transmission = dto.Transmission,
                CreatedBy = user,
                City = dto.City,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
            };

            car.Images = dto.ImageUrls
            .Select(x => new CarImages{ Car = car, ImageUrl = x })
            .ToList();

            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "OK";
            response.StatusCode = StatusCodes.Status200OK;
            return response;

        }
        public async Task<ServiceResponse<bool>> UpdateCarAsync(UpdateCarDTO dto, int userId)
        {
            var response = new ServiceResponse<bool>();

            var car = await _context.Cars.Include(x => x.CreatedBy).Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == dto.CarId);

            if (car == null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Car not found!";
                response.StatusCode = StatusCodes.Status404NotFound;
                return response;
            }

            if (car.CreatedBy.Id != userId)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "This car doesn't belong to you!";
                response.StatusCode = StatusCodes.Status400BadRequest;

                return response;
            }

            if (!string.IsNullOrEmpty(dto.Brand))
                car.Brand = dto.Brand;

            if(!string.IsNullOrEmpty(dto.Model))
                car.Model = dto.Model;

            if(dto.ReleaseDate.HasValue)
                car.ReleaseDate = dto.ReleaseDate.Value;

            if (dto.Images != null && dto.Images.Count > 0)
                car.Images = dto.Images
                    .Select(x => new CarImages 
                    {
                        Car = car,
                        ImageUrl = x
                    }).ToList();

            if(dto.Price != null)
                car.Price = (decimal)dto.Price;

            if(dto.EngineCapacity != null)
                car.EngineCapacity = (double)dto.EngineCapacity;

            if(!string.IsNullOrEmpty(dto.Transmission))
                car.Transmission = dto.Transmission;

            if (!string.IsNullOrEmpty(dto.City))
                car.City = dto.City;

            if (!string.IsNullOrEmpty(dto.Latitude))
                car.Latitude = dto.Latitude;

            if (!string.IsNullOrEmpty(dto.Longitude))
                car.Longitude = dto.Longitude;

            _context.Cars.Update(car);

            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = true;
            response.Message = $"{car.Brand} {car.Model} successfully updated";
            response.StatusCode = StatusCodes.Status200OK;

            return response;
        }
        public async Task<ServiceResponse<List<GetFilteredCarDTO>>> GetFilteredCarsAsync(CarFilterDTO dto)
        {
            var response = new ServiceResponse<List<GetFilteredCarDTO>>();

            IQueryable<Car> query = _context.Cars.Include(x => x.CreatedBy).Include(x => x.Images);

            
            if (!string.IsNullOrEmpty(dto.Brand))
                query = query.Where(x => x.Brand.ToLower() == dto.Brand.ToLower());

            if (!string.IsNullOrEmpty(dto.Model))
                query = query.Where(x => x.Model.ToLower() == dto.Model.ToLower());

            if (dto.DateFrom.HasValue)
                query = query.Where(x => x.ReleaseDate >= dto.DateFrom);

            if (dto.DateTo.HasValue)
                query = query.Where(x => x.ReleaseDate <= dto.DateTo);

            if (dto.PriceFrom.HasValue)
                query = query.Where(x => x.Price >= dto.PriceFrom);

            if (dto.PriceTo.HasValue)
                query = query.Where(x => x.Price <= dto.PriceTo);

            if (dto.EngineCapacityFrom.HasValue)
                query = query.Where(x => x.EngineCapacity >= dto.EngineCapacityFrom);

            if (dto.EngineCapacityTo.HasValue)
                query = query.Where(x => x.EngineCapacity <= dto.EngineCapacityTo);

            if (!string.IsNullOrEmpty(dto.Transmission))
                query = query.Where(x => x.Transmission.ToLower() == dto.Transmission.ToLower());

            if (!string.IsNullOrEmpty(dto.City))
                query = query.Where(x => x.City.ToLower() == dto.City.ToLower());

            var filteredCars = await query
                .Select(car => new GetFilteredCarDTO
                {
                    Id = car.Id,
                    Brand = car.Brand,
                    Model = car.Model,
                    ReleaseDate = car.ReleaseDate,
                    Price = car.Price,
                    EngineCapacity = car.EngineCapacity,
                    Transmission = car.Transmission,
                    City = car.City,
                    UserPhoneN = car.CreatedBy.Mobile,
                    UserEmail = car.CreatedBy.Email,
                    UserFirstName = car.CreatedBy.FirstName,
                    UserLasttName = car.CreatedBy.LastName,
                    ImageUrls = car.Images.Select(x => x.ImageUrl).ToList(),
                })
                .ToListAsync();

            response.Success = true;
            response.StatusCode = StatusCodes.Status200OK;
            response.Data = filteredCars;

            return response;

        }

        public async Task<ServiceResponse<GetCarDTO>> GetCarAsync(int id)
        {
            var response = new ServiceResponse<GetCarDTO>();

            var car = await _context.Cars.Include(x => x.Images).Include(x => x.CreatedBy).FirstOrDefaultAsync(x => x.Id == id);

            if (car == null)
            {
                response.Success = false;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Data = null;
                return response;
            }

            var carDto = new GetCarDTO()
            {
                Brand = car.Brand,
                Model = car.Model,
                ReleaseDate = car.ReleaseDate,
                ImageUrls = car.Images.Select(x => x.ImageUrl).ToList(),
                Price = car.Price,
                EngineCapacity = car.EngineCapacity,
                Transmission = car.Transmission,
                UserFirstName = car.CreatedBy.FirstName,
                UserLasttName = car.CreatedBy.LastName,
                UserEmail = car.CreatedBy.Email,
                UserPhoneN = car.CreatedBy.Mobile,
                City = car.City,
                Latitude = car.Latitude,
                Longitude = car.Longitude,
            };

            response.Data = carDto;
            response.Success = true;
            response.StatusCode = StatusCodes.Status200OK;

            return response;
        }

        public async Task<ServiceResponse<List<GetCarDTO>>> GetPaginatedCarsAsync(int page, int numberOfCars)
        {
            var response = new ServiceResponse<List<GetCarDTO>>();

            if (page < 1) page = 1;
            if (numberOfCars < 1) numberOfCars = 10;

            IQueryable<Car> query = _context.Cars.Include(x => x.CreatedBy).Include(x => x.Images);

            int totalCars = await query.CountAsync();

            var paginatedCars = await query
                .Skip((page - 1) * numberOfCars)
                .Take(numberOfCars)
                .Select(car => new GetCarDTO
                {
                    Id = car.Id,
                    Brand = car.Brand,
                    Model = car.Model,
                    ReleaseDate = car.ReleaseDate,
                    Price = car.Price,
                    EngineCapacity = car.EngineCapacity,
                    Transmission = car.Transmission,
                    City = car.City,
                    UserPhoneN = car.CreatedBy.Mobile,
                    UserEmail = car.CreatedBy.Email,
                    UserFirstName = car.CreatedBy.FirstName,
                    UserLasttName = car.CreatedBy.LastName,
                    ImageUrls = car.Images.Select(x => x.ImageUrl).ToList(),
                })
                .ToListAsync();

            response.Success = true;
            response.Data = paginatedCars;
            response.Message = $"Page {page} of {Math.Ceiling((double)totalCars / numberOfCars)}";

            return response;
        }

    }
}
