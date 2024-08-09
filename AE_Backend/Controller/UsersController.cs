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

namespace AE_Backend.Controllers
{

    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("api/users/GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
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

        [HttpGet("api/users/GetUserbyId/{id:int}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);

                if (user == null)
                {
                    return NotFound("User not found or inactive.");
                }

                return user;
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

        [HttpPost("api/users/CreateUser")]
        public IActionResult CreateUser([FromBody] UserCreateParam userDto)
        {
            try
            {
                int userId = _userService.InsertUser(userDto);
                if (userId == 0)
                {
                    return BadRequest(new { status = "error", message = "Failed to create user." });
                }
                return Ok(new { status = "success", userId = userId });
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

        [HttpPost("api/users/UpdateUser")]
        public IActionResult UpdateUser([FromBody] UserUpdateParam userDto)
        {
            try
            {
                User userData = _userService.UpdateUser(userDto);
                return Ok(new { status = "success", userData = userData });
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

        [HttpPost("api/users/DeleteUser")]
        public async Task<IActionResult> DeleteUser(int userId, string modifiedBy)
        {
            try
            {
                string result = await _userService.DeleteUser(userId, modifiedBy);
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

        [HttpGet("api/users/GetShipsForUser/{userId}")]
        public async Task<IActionResult> GetShipsForUser(int userId)
        {
            try
            {
                var userShips = await _userService.GetShipsForUser(userId);

                return Ok(userShips);
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
