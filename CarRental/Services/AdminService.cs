using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Models.DTOs.Rental;
using CarRental.Models.DTOs.User;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<UserGetDTO>>> GetUsersAsync()
        {
            var response = new ServiceResponse<List<UserGetDTO>>();

            var users = await _context.Users
                .Include(x => x.Roles)
                .ToListAsync();

            var usersDto = users.Select(x => new UserGetDTO()
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Mobile = x.Mobile,
                Roles = x.Roles?
                .Select(r => r.Name)
                .ToList() ?? new List<string>()
            }).ToList();

            response.Success = true;
            response.Data = usersDto;
            response.Message = "All users";

            return response;
        }
        public async Task<ServiceResponse<bool>> VerifyUserAsync(int userId)
        {
            var response = new ServiceResponse<bool>();

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "User not found";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;
            }

            if (user.IsVerified)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "User already verified";
                response.StatusCode= StatusCodes.Status400BadRequest;
                return response;
            }

            user.IsVerified = true;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = true;
            response.Message = "User verified";
            return response;
        }

        public async Task<ServiceResponse<GetActiveRentalsResponseDTO>> GetActiveRentalsAsync()
        {
            var response = new ServiceResponse<GetActiveRentalsResponseDTO>();

            var rentals = await _context.Rentals
                .Include(x=> x.User)
                .Include(x => x.Car)
                .ThenInclude(x => x.CreatedBy)
                .Where(x => x.Till >= DateOnly.FromDateTime(DateTime.Today)).ToListAsync();

            if (!rentals.Any())
            {
                response.Success = false;
                response.Message = "No Active Rentals";
                response.StatusCode = StatusCodes.Status400BadRequest;

                return response;
            }

            var dto = rentals.Select(x => new GetActiveRentalDTO()
            {
                RentalId = x.Id,
                OwnerName = x.Car.CreatedBy.FirstName + " " + x.Car.CreatedBy.LastName,
                OwnerEmail = x.Car.CreatedBy.Email,
                CarManufacturer = x.Car.Brand,
                CarModel = x.Car.Model,
                RenterName = x.User.FirstName + " " + x.User.LastName,
                RenterEmail = x.User.Email,
                RentalFee = x.TotalFee,
                StartDate = x.From,
                EndDate = x.Till,
                RentalDuration = x.DaysRented
            }).ToList();

            var averageRentalFee = rentals
                .Select(x => x.TotalFee)
                .Average();

            response.Success = true;
            response.Message = "All active rentals and average rental fee";
            response.Data = new GetActiveRentalsResponseDTO
            {
                Rentals = dto,
                AverageRentalFee = averageRentalFee
            };

            return response;
        }


    }
}
