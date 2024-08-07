using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AE_Backend.db;
using AE_Backend.Model;
using Microsoft.Data.SqlClient;
using AE_Backend.Services;

namespace AE_Backend.Controller
{
    [ApiController]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRolesController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet("api/UserRoles/GetAllUserRoles")]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetAllUserRoles()
        {
            try
            {
                var userRoles = await _userRoleService.GetAllUserRoles();
                return Ok(userRoles);
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }

        [HttpGet("api/UserRoles/GetUserRolebyId/{id:int}")]
        public async Task<ActionResult<UserRole>> GetUserRole(int id)
        {
            try
            {
                var userRole = await _userRoleService.GetUserRoleById(id);

                if (userRole == null)
                {
                    return NotFound("UserRole not found or inactive.");
                }

                return userRole;
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }

        [HttpPost("api/UserRoles/CreateUserRole")]
        public async Task<IActionResult> CreateUserRole([FromBody] UserRoleCreateParam userRoleDto)
        {
            try
            {
                int userId = await _userRoleService.InsertUserRole(userRoleDto);
                return Ok(new { status = "success", userId = userId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }

        [HttpPost("api/UserRoles/UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UserRoleUpdateParam userRoleDto)
        {
            try
            {
                UserRole userRoleData = await _userRoleService.UpdateUserRole(userRoleDto);
                return Ok(new { status = "success", userRoleData = userRoleData });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }

        [HttpPost("api/UserRoles/DeleteUserRole")]
        public async Task<IActionResult> DeleteUserRole(int userRoleId, string modifiedBy)
        {
            try
            {
                string result = await _userRoleService.DeleteUserRole(userRoleId, modifiedBy);
                return Ok(new { status = "success", result = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }
    }
}
