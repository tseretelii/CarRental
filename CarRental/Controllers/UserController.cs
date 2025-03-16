using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Models.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ServiceResponse<bool>> RegisterAsync(UserRegisterDTO dto)
        {
            return await _userService.RegisterAsync(dto);
        }
    }
}
