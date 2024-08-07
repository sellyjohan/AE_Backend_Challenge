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
                return Ok(userShips);
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
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
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }

        [HttpPost("api/UserShips/CreateUserShip")]
        public async Task<IActionResult> CreateUserShip([FromBody] UserShipCreateParam userShipDto)
        {
            try
            {
                int userShipId = await _userShipService.InsertUserShip(userShipDto);
                return Ok(new { status = "success", userShipId = userShipId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }

        [HttpPost("api/UserShips/UpdateUserShip")]
        public async Task<IActionResult> UpdateUserShip([FromBody] UserShipUpdateParam userShipDto)
        {
            try
            {
                UserShip userShipData = await _userShipService.UpdateUserShip(userShipDto);
                return Ok(new { status = "success", userShipData = userShipData });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
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
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }
    }
}
