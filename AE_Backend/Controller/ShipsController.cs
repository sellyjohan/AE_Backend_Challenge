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
using AE_Backend.General;

namespace AE_Backend.Controller
{
    [ApiController]
    public class ShipsController : ControllerBase
    {
        private readonly IShipService _shipService;
        private readonly Utility _utility;
        public ShipsController(IShipService shipService, Utility utility)
        {
            _shipService = shipService;
            _utility = utility;
        }

        [HttpGet("api/ships/GetAllShips")]
        public async Task<ActionResult<IEnumerable<Ship>>> GetAllUsers()
        {
            try
            {
                var ships = await _shipService.GetAllShips();

                if (ships == null || ships.Count() == 0)
                {
                    return NotFound("No active ship found in the database.");
                }

                return Ok(ships);
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

        [HttpGet("api/ships/GetShipbyId/{id:int}")]
        public async Task<ActionResult<Ship>> GetShip(int id)
        {
            try
            {
                var ship = await _shipService.GetShipById(id);

                if (ship == null)
                {
                    return NotFound("Ship not found or inactive.");
                }

                return ship;
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

        [HttpPost("api/ships/CreateShip")]
        public IActionResult CreateShip([FromBody] ShipCreateParam userDto)
        {
            try
            {
                int shipId = _shipService.InsertShip(userDto);
                if (shipId == 0)
                {
                    return BadRequest(new { status = "error", message = "Failed to create ship." });
                }
                return Ok(new { status = "success", shipId = shipId });
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

        [HttpPost("api/ships/UpdateShip")]
        public IActionResult UpdateShip([FromBody] ShipUpdateParam shipDto)
        {
            try
            {
                Ship shipData = _shipService.UpdateShip(shipDto);
                return Ok(new { status = "success", shipData = shipData });
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

        [HttpPost("api/ships/DeleteShip")]
        public async Task<IActionResult> DeleteShip(int shipId, string modifiedBy)
        {
            try
            {
                string result = await _shipService.DeleteShip(shipId, modifiedBy);
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


        [HttpGet("api/ships/GetUnassignedShips")]
        public async Task<IActionResult> GetUnassignedShips()
        {
            try
            {
                var ships = await _shipService.GetUnassignedShips();

                return Ok(ships);
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

        [HttpGet("api/ships/GetClosestPort/{shipId}")]
        public async Task<IActionResult> GetClosestPort(int shipId)
        {
            try
            {
                var ship = await _shipService.GetShipById(shipId);
                if (ship == null)
                    return NotFound($"Ship with ID {shipId} not found.");

                var (closestPort, estimatedTime) = await _shipService.CalculateClosestPort(ship);

                if (closestPort == null)
                    return NotFound("No ports found.");

                decimal distanceKm = _utility.CalculateDistance(ship.Latitude, ship.Longitude, closestPort.Latitude, closestPort.Longitude);

                return Ok(new
                {
                    ClosestPort = closestPort,
                    EstimatedArrivalTime = estimatedTime,
                    DistanceKm = distanceKm
                });
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

        [HttpGet("api/ships/port-distances/{shipId}/{limit}")]
        public async Task<IActionResult> GetAllPortDistancesAndTimes(int shipId, int limit)
        {
            try
            {
                var ship = await _shipService.GetShipById(shipId);
                if (ship == null)
                    return NotFound($"Ship with ID {shipId} not found.");

                var closestPorts = await _shipService.GetAllPortDistancesAndTimes(ship, limit);

                if (closestPorts == null || closestPorts.Count == 0)
                    return NotFound("No ports found.");

                return Ok(closestPorts);
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
