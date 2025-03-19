using Azure;
using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Models.DTOs.Token;
using CarRental.Models.DTOs.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

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

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<bool>>> RegisterAsync(UserRegisterDTO dto)
        {
            var response = await _userService.RegisterAsync(dto);

            if (response.StatusCode >= 400 && response.StatusCode < 500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<TokenDTO>>> LoginAsync(UserLoginDTO dto)
        {
            var response = await _userService.LoginAsync(dto);

            if (response.StatusCode >= 400 && response.StatusCode <500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);

        }
        [HttpPut("VerifyAccount")]
        public async Task<ActionResult<ServiceResponse<bool>>> VerifyAccountAsync(string telNum, string code)
        {
            var response = await _userService.VerifyAccountAsync(telNum, code);

            if(response.StatusCode >= 400 && response.StatusCode < 500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
        [HttpPatch("ResendVerificationCode")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ResendVerificationCode()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userId, out int Id))
            {
                var response = await _userService.ResendVerificationCode(Id);

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
