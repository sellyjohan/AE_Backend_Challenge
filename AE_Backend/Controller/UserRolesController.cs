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

                if (userRoles == null)
                {
                    return NotFound("No active user roles found in the database.");
                }

                return Ok(userRoles);
            }
            catch (TimeoutException)
            {
                return StatusCode(504, new { status = "error", message = "Request timed out." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
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
                    return NotFound("User role not found or inactive.");
                }

                return userRole;
            }
            catch (TimeoutException)
            {
                return StatusCode(504, new { status = "error", message = "Request timed out." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        [HttpPost("api/UserRoles/CreateUserRole")]
        public IActionResult CreateUserRole([FromBody] UserRoleCreateParam userRoleDto)
        {
            try
            {
                int userRoleId = _userRoleService.InsertUserRole(userRoleDto);
                if (userRoleId == 0)
                {
                    return BadRequest(new { status = "error", message = "Failed to create user role." });
                }
                return Ok(new { status = "success", userRoleId = userRoleId });
            }
            catch (DbUpdateException dbEx)
            {
                var detailedMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, new { status = "error", message = detailedMessage });
            }
            catch (TimeoutException)
            {
                return StatusCode(504, new { status = "error", message = "Request timed out." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }

        [HttpPost("api/UserRoles/UpdateUserRole")]
        public IActionResult UpdateUserRole([FromBody] UserRoleUpdateParam userRoleDto)
        {
            try
            {
                UserRole userRoleData = _userRoleService.UpdateUserRole(userRoleDto);
                return Ok(new { status = "success", userRoleData = userRoleData });
            }
            catch (DbUpdateException dbEx)
            {
                var detailedMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, new { status = "error", message = detailedMessage });
            }
            catch (TimeoutException)
            {
                return StatusCode(504, new { status = "error", message = "Request timed out." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
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
            catch (TimeoutException)
            {
                return StatusCode(504, new { status = "error", message = "Request timed out." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }
    }
}
