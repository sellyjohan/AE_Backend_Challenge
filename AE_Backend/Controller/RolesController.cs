using AE_Backend.Model;
using AE_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AE_Backend.Controller
{
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("api/Roles/GetAllRoles")]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRoles();
                return Ok(roles);
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

        [HttpGet("api/Roles/GetRolebyId/{id:int}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            try
            {
                var role = await _roleService.GetRoleById(id);

                if (role == null)
                {
                    return NotFound("Role not found or inactive.");
                }

                return role;
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

        [HttpPost("api/Roles/CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateParam roleDto)
        {
            try
            {
                int roleId = await _roleService.InsertRole(roleDto);
                if (roleId == 0)
                {
                    return BadRequest(new { status = "error", message = "Failed to create role." });
                }
                return Ok(new { status = "success", roleId = roleId });
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

        [HttpPost("api/Roles/UpdateRole")]
        public IActionResult UpdateRole([FromBody] RoleUpdateParam roleDto)
        {
            try
            {
                Role roleData = _roleService.UpdateRole(roleDto);
                return Ok(new { status = "success", roleData = roleData });
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

        [HttpPost("api/Roles/DeleteRole")]
        public async Task<IActionResult> DeleteRole(int roleId, string modifiedBy)
        {
            try
            {
                string result = await _roleService.DeleteRole(roleId, modifiedBy);
                return Ok(new { status = "success", result = result });
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
    }
    
}
