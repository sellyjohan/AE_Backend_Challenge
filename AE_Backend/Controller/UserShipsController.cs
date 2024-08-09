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
    public class UserShipsController : ControllerBase
    {
        private readonly IUserShipService _userShipService;

        public UserShipsController(IUserShipService userShipService)
        {
            _userShipService = userShipService;
        }

        [HttpGet("api/UserShips/GetAllUserShips")]
        public async Task<ActionResult<IEnumerable<UserShip>>> GetAllUserShips()
        {
            try
            {
                var userShips = await _userShipService.GetAllUserShips();

                if (userShips == null || userShips.Count() == 0)
                {
                    return NotFound("No active user ship found in the database.");
                }

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

        [HttpGet("api/UserShips/GetUserShipbyId/{id:int}")]
        public async Task<ActionResult<UserShip>> GetUserShip(int id)
        {
            try
            {
                var userShip = await _userShipService.GetUserShipById(id);

                if (userShip == null)
                {
                    return NotFound("UserShip not found or inactive.");
                }

                return userShip;
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

        [HttpPost("api/UserShips/CreateUserShip")]
        public IActionResult CreateUserShip([FromBody] UserShipCreateParam userShipDto)
        {
            try
            {
                int userShipId = _userShipService.InsertUserShip(userShipDto);
                return Ok(new { status = "success", userShipId = userShipId });
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

        [HttpPost("api/UserShips/UpdateUserShip")]
        public IActionResult UpdateUserShip([FromBody] UserShipUpdateParam userShipDto)
        {
            try
            {
                UserShip userShipData = _userShipService.UpdateUserShip(userShipDto);
                return Ok(new { status = "success", userShipData = userShipData });
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

        [HttpPost("api/UserShips/DeleteUserShip")]
        public async Task<IActionResult> DeleteUserShip(int userShipId, string modifiedBy)
        {
            try
            {
                string result = await _userShipService.DeleteUserShip(userShipId, modifiedBy);
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
