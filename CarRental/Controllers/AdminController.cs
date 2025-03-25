using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Models.DTOs.Rental;
using CarRental.Models.DTOs.User;
using CarRental.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("GetUsers")]
        public async Task<ActionResult<ServiceResponse<List<UserGetDTO>>>> GetUsersAsync()
        {
            var response = await _adminService.GetUsersAsync();

            if (response.StatusCode >= 400 && response.StatusCode < 500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
        [HttpPatch("VerifyUser/{userId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> VerifyUserAsync(int userId)
        {
            var response = await _adminService.VerifyUserAsync(userId);

            if (response.StatusCode >= 400 && response.StatusCode < 500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
        [HttpGet("GetActiveRentals")]
        public async Task<ActionResult<ServiceResponse<GetActiveRentalsResponseDTO>>> GetActiveRentalsAsync()
        {
            var response = await _adminService.GetActiveRentalsAsync();

            if (response.StatusCode >= 400 && response.StatusCode < 500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
    }
}
