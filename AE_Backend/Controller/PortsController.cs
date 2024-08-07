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
    public class PortsController : ControllerBase
    {
        private readonly IPortService _portService;

        public PortsController(IPortService portService)
        {
            _portService = portService;
        }

        [HttpGet("api/ports/GetAllPorts")]
        public async Task<ActionResult<IEnumerable<Port>>> GetAllUsers()
        {
            try
            {
                var users = await _portService.GetAllPorts();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }

        [HttpGet("api/ports/GetPortbyId/{id:int}")]
        public async Task<ActionResult<Port>> GetPort(int id)
        {
            try
            {
                var port = await _portService.GetPortById(id);

                if (port == null)
                {
                    return NotFound("Port not found or inactive.");
                }

                return port;
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "error", message = ex.Message });
            }
        }
    }
}
