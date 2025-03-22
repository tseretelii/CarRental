using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Models.DTOs.Car;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpPost("RegisterCar")]
        [Authorize(Roles = "verified")]
        public async Task<ActionResult<ServiceResponse<bool>>> RegisterCarAsync(RegisterCarDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userId, out int Id))
            {
                var response = await _carService.RegisterCarAsync(dto, Id);

                if (response.StatusCode >= 400 && response.StatusCode < 500)
                    return BadRequest(response);
                else if (response.StatusCode >= 500 && response.StatusCode < 600)
                    return StatusCode(response.StatusCode, response);

                return Ok(response);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        [HttpPut("UpdateCar")]
        [Authorize(Roles = "verified")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateCarAsync(UpdateCarDTO dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userId, out int Id))
            {
                var response = await _carService.UpdateCarAsync(dto, Id);

                if (response.StatusCode >= 400 && response.StatusCode < 500)
                    return BadRequest(response);
                else if (response.StatusCode >= 500 && response.StatusCode < 600)
                    return StatusCode(response.StatusCode, response);

                return Ok(response);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        [HttpPost("GetFilteredCars")]
        public async Task<ActionResult<ServiceResponse<List<GetFilteredCarDTO>>>> GetFilteredCarsAsync([FromForm] CarFilterDTO dto)
        {
            var response = await _carService.GetFilteredCarsAsync(dto);

            if (response.StatusCode >= 400 && response.StatusCode < 500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
        [HttpGet("GetCar/{Id}")]
        public async Task<ActionResult<GetCarDTO>> GetCarAsync(int Id)
        {
            var response = await _carService.GetCarAsync(Id);

            if (response.StatusCode >= 400 && response.StatusCode < 500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
        [HttpGet("GetPaginatedCars/{page}/{numberOfCars}")]
        public async Task<ActionResult<ServiceResponse<List<GetCarDTO>>>> GetPaginatedCarsAsync(int page, int numberOfCars)
        {
            var response = await _carService.GetPaginatedCarsAsync(page, numberOfCars);

            if (response.StatusCode >= 400 && response.StatusCode < 500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
    }
}
