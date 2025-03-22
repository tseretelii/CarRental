using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Models.DTOs.Role;
using CarRental.Models.DTOs.RoleUser;
using CarRental.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<ServiceResponse<List<GetRoleDTO>>>> GetAllRolesAsync()
        {
            var response = await _roleService.GetAllRolesAsync();

            if (response.StatusCode >= 400 && response.StatusCode < 500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
        [HttpPost("CreateRole")]
        public async Task<ActionResult<ServiceResponse<bool>>> CreateRoleAsync(CreateRoleDTO dto)
        {
            var response = await _roleService.CreateRoleAsync(dto);

            if (response.StatusCode >= 400 && response.StatusCode < 500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
        [HttpPut("UpdateRole")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateRoleAsync(UpdateRoleDTO dto)
        {
            var response = await _roleService.UpdateRoleAsync(dto);

            if (response.StatusCode >= 400 && response.StatusCode < 500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
        [HttpDelete("DeleteRole/{Id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteRoleAsync(int Id)
        {
            var response = await _roleService.DeleteRoleAsync(Id);

            if (response.StatusCode >= 400 && response.StatusCode < 500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
        [HttpPost("AddRoleToUser")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddRoleToUserAsync(AddRoleToUserDTO dto)
        {
            var response = await _roleService.AddRoleToUserAsync(dto);

            if (response.StatusCode >= 400 && response.StatusCode < 500)
                return BadRequest(response);
            else if (response.StatusCode >= 500 && response.StatusCode < 600)
                return StatusCode(response.StatusCode, response);

            return Ok(response);
        }
    }
}
