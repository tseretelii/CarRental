using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "verified")]
    public class FavoriteCarController : ControllerBase
    {
        private readonly IFavoriteCarService _service;

        public FavoriteCarController(IFavoriteCarService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> AddToFavorite(int carId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userId, out int Id))
            {
                var response = await _service.AddCarToFavoritesAsync(Id, carId);

                if (response.StatusCode >= 400 && response.StatusCode < 500)
                    return BadRequest(response);
                else if (response.StatusCode >= 500 && response.StatusCode < 600)
                    return StatusCode(response.StatusCode, response);

                return Ok(response);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);

        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveCarFromFavoritesAsync(int carId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userId, out int Id))
            {
                var response = await _service.RemoveCarFromFavoritesAsync(Id, carId);

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
