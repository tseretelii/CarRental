using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Models.DTOs.Rental;
using CarRental.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Services
{
    public class RentalService : IRentalService
    {
        private readonly ApplicationDbContext _context;

        public RentalService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<bool>> RentCarAsync(int userId, RentCarDTO dto)
        {
            var response = new ServiceResponse<bool>();
            
            if (dto.From < DateOnly.FromDateTime(DateTime.Today))
            {
                response.Success = false;
                response.Message = "Rental can't begin in the past!";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;
            }

            if (dto.To < dto.From)
            {
                response.Success = false;
                response.Message = "Incorrect duration input!";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                response.Success = false;
                response.Message = "Invalis user";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;
            }

            var car = await _context.Cars.Include(x => x.CreatedBy).FirstOrDefaultAsync(x => x.Id == dto.CarId);

            if (car == null)
            {
                response.Success = false;
                response.Message = "Car not found!";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;
            }

            if (car.CreatedBy.Id == userId)
            {
                response.Success = false;
                response.Message = "Can't rent your own car!";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return response;
            }

            var checkCar = await _context.Rentals.AnyAsync(x =>
                x.Car.Id == car.Id &&
                x.From <= dto.To &&
                x.Till >= dto.From);

            if (checkCar)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "In these dates, car is occupied";
                response.StatusCode= StatusCodes.Status400BadRequest;

                return response;
            }

            var daysRented = dto.To.DayNumber - dto.From.DayNumber + 1;

            var rental = new Rental()
            {
                Car = car,
                User = user,
                DaysRented = daysRented,
                From = dto.From,
                Till = dto.To,
                TotalFee = car.Price * daysRented
            };

            await _context.Rentals.AddAsync(rental);

            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = true;
            response.Message = "Car rented";

            try
            {
                string emailBodyForOwner = $@"
                <p>Dear {car.CreatedBy.FirstName},</p>
                <p>We are pleased to inform you that your car has been successfully rented!</p>
                <p><strong>Rental Details:</strong></p>
                <ul>
                    <li><strong>Car:</strong> {car.Brand} {car.Model}</li>
                    <li><strong>Rental Date:</strong> {rental.From:MMMM dd, yyyy} - {rental.Till:MMMM dd, yyyy}</li>
                    <li><strong>Renter:</strong> {user.FirstName} {user.LastName} - Emai: {user.Email}; Phone: {user.Mobile}</li>
                    <li><strong>Total Fee:</strong> ${rental.TotalFee}</li>
                </ul>
                <p>If you have any questions or need further assistance, feel free to contact us.</p>
                <p>Best regards,</p>
                <p><strong>Car Rental Team</strong></p>
                ";

                HelperServices.SendEmail(car.CreatedBy.Email, car.CreatedBy.FirstName, "Your Car Has Been Rented!", emailBodyForOwner); // owner of the car

                string emailBodyForCustomer = $@"
                <p>Dear {user.FirstName},</p>
                <p>Congratulations! Your car rental request has been successfully processed.</p>
                <p><strong>Rental Details:</strong></p>
                <ul>
                    <li><strong>Car:</strong> {car.Brand} {car.Model}</li>
                    <li><strong>Rental Period:</strong> {rental.From:MMMM dd, yyyy} - {rental.Till:MMMM dd, yyyy}</li>
                    <li><strong>Pickup Location:</strong> {car.City}</li>
                    <li><strong>Total Fee:</strong> ${rental.TotalFee}</li>
                </ul>
                <p>Please make sure to review the rental terms and pick up the vehicle on time.</p>
                <p>If you have any questions or need assistance, feel free to contact us.</p>
                <p>Safe travels and enjoy your ride!</p>
                <p><strong>Best regards,</strong></p>
                <p><strong>Car Rental Team</strong></p>
                ";
                HelperServices.SendEmail(user.Email, user.FirstName, "Car rental request has been successfully processed", emailBodyForCustomer); // renter
            }
            catch (Exception ex)
            {
                HelperServices.WriteExceptionToFile(ex, "RentalService");
                return response;
            }

            return response;
        }
    }
}
