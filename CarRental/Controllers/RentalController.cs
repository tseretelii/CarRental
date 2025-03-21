using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Models.DTOs.Rental;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "verified")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalController(IRentalService service)
        {
            _rentalService = service;
        }

        [HttpPost("RentCar")]
        public async Task<ActionResult<ServiceResponse<bool>>> RentCarAsync(RentCarDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userId, out int Id))
            {
                var response = await _rentalService.RentCarAsync(Id, dto);

                if (response.StatusCode >= 400 && response.StatusCode < 500)
                    return BadRequest(response);
                else if (response.StatusCode >= 500 && response.StatusCode < 600)
                    return StatusCode(response.StatusCode, response);

                return Ok(response);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
