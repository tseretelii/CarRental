using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CarRental.Interfaces;
using CarRental.Models;
using System.Security.Claims;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "verified")]
    public class FavoriteCarController : ControllerBase
    {
        private readonly IUserService _userService;

        public FavoriteCarController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> AddCarToFavorites(int carId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userId, out int Id))
            {
                var response = await _userService.AddCarToFavorites(carId, Id);

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
