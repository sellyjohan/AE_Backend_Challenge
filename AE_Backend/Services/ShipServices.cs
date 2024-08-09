using AE_Backend.db;
using AE_Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using AE_Backend.General;

namespace AE_Backend.Services
{
    public interface IShipService
    {
        int InsertShip(ShipCreateParam shipDto);
        Task<IEnumerable<Ship>> GetAllShips();
        Task<Ship> GetShipById(int userId);
        Ship UpdateShip(ShipUpdateParam shipDto);
        Task<string> DeleteShip(int shipId, string modifiedBy);
        Task<IEnumerable<Ship>> GetUnassignedShips();
        Task<(Port? closestPort, TimeSpan estimatedTime)> CalculateClosestPort(Ship ship);
        Task<List<PortInfo>> GetAllPortDistancesAndTimes(Ship ship, int limit);
    }
    public class ShipServices : IShipService
    {
        private readonly MyDbContext _dbContext;
        private readonly Utility _utility;

        public ShipServices(MyDbContext dbContext, Utility utility)
        {
            _dbContext = dbContext;
            _utility = utility;
        }

        public int InsertShip(ShipCreateParam shipDto)
        {
            try
            {
                var result = _dbContext.Ships
                    .FromSqlRaw("EXEC [dbo].[SP_InsertShip] @shipname, @longitude, @latitude, @velocity, @createdby",
                        new SqlParameter("@shipname", shipDto.ShipName),
                        new SqlParameter("@longitude", shipDto.Longitude),
                        new SqlParameter("@latitude", shipDto.Latitude),
                        new SqlParameter("@velocity", shipDto.Velocity),
                        new SqlParameter("@createdby", shipDto.CreatedBy))
                    .AsEnumerable()
                    .Select(u => u.ShipId)
                    .FirstOrDefault();

                if (result == 0)
                {
                    throw new DbUpdateException("Error creating ship: Failed to create ship.");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting ship: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Ship>> GetAllShips()
        {
            try
            {
                var result = await _dbContext.Ships
                    .Where(r => r.RowStatus == 1)
                    .ToListAsync();

                if (result == null || result.Count == 0)
                {
                    throw new CustomException.ShipNotFoundException($"No active ship found.");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving ships: {ex.Message}");
            }
        }

        public async Task<Ship> GetShipById(int shipId)
        {
            try
            {
                var result = await _dbContext.Ships
                    .Where(u => u.ShipId == shipId && u.RowStatus == 1)
                    .FirstOrDefaultAsync();

                if (result == null)
                {
                    throw new CustomException.ShipNotFoundException($"Ship with ID {shipId} not found.");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving ship with ID {shipId}: {ex.Message}");
            }
        }


        public Ship UpdateShip(ShipUpdateParam shipDto)
        {
            try
            {
                var result = _dbContext.Ships
                .FromSqlRaw("EXEC [dbo].[SP_UpdateShip] @shipid, @shipname, @longitude, @latitude, @velocity, @isActive, @modifiedby",
                    new SqlParameter("@shipid", shipDto.ShipId),
                    new SqlParameter("@shipname", shipDto.ShipName),
                    new SqlParameter("@longitude", shipDto.Longitude),
                    new SqlParameter("@latitude", shipDto.Latitude),
                    new SqlParameter("@velocity", shipDto.Velocity),
                    new SqlParameter("@isActive", shipDto.RowStatus),
                    new SqlParameter("@modifiedby", shipDto.ModifiedBy))
                .AsEnumerable()
                .FirstOrDefault();

                if(result == null)
                {
                    throw new DbUpdateException("Error updating ship: Result is null.");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error update ship: {ex.Message}");
            }
        }

        public async Task<string> DeleteShip(int shipId, string modifiedBy)
        {
            try
            {
                var result = await _dbContext.Database
                    .ExecuteSqlRawAsync("EXEC [dbo].[SP_DeleteShip] @shipid, @modifiedby",
                        new SqlParameter("@shipid", shipId),
                        new SqlParameter("@modifiedby", modifiedBy));

                if (result is int rowsAffected && rowsAffected > 0)
                {
                    return "failed: No rows deleted.";
                }
                else
                {
                    return "success: Data has been deleted";
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting ship: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Ship>> GetUnassignedShips()
        {
            try
            {
                return await _dbContext.Ships
                .Where(s => !_dbContext.UserShips.Any(us => us.ShipId == s.ShipId && us.RowStatus == 1) && s.RowStatus == 1)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving unassigned ship: {ex.Message}");
            }
        }

        public async Task<(Port? closestPort, TimeSpan estimatedTime)> CalculateClosestPort(Ship ship)
        {
            try
            {
                var allPorts = await _dbContext.Ports.ToListAsync();

                if (allPorts == null || !allPorts.Any())
                {
                    throw new CustomException.PortNotFoundException("Error retrieving port: Result is null.");
                    //return (null, TimeSpan.Zero);
                }

                Port? closestPort = null;
                decimal minDistance = decimal.MaxValue;

                foreach (var port in allPorts)
                {
                    decimal distance = _utility.CalculateDistance(ship.Latitude, ship.Longitude, port.Latitude, port.Longitude);
                    decimal estimatedTimeHours = distance / ship.Velocity;

                    if (estimatedTimeHours < minDistance)
                    {
                        minDistance = estimatedTimeHours;
                        closestPort = port;
                    }
                }

                // Convert estimated time from hours to TimeSpan
                TimeSpan estimatedTime = TimeSpan.FromHours((double)minDistance);

                return (closestPort, estimatedTime);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException($"Error retrieving port: Result is null. {dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving closest port: {ex.Message}");
            }
            
        }

        public async Task<List<PortInfo>> GetAllPortDistancesAndTimes(Ship ship, int limit)
        {
            try
            {
                var allPorts = await _dbContext.Ports.ToListAsync();
                var portInfos = new List<PortInfo>();

                foreach (var port in allPorts)
                {
                    decimal distance = _utility.CalculateDistance(ship.Latitude, ship.Longitude, port.Latitude, port.Longitude);
                    decimal estimatedTimeHours = distance / ship.Velocity;

                    var portInfo = new PortInfo
                    {
                        Port = port,
                        DistanceKm = distance,
                        EstimatedArrivalTime = TimeSpan.FromHours((double)estimatedTimeHours)
                    };

                    portInfos.Add(portInfo);
                }

                var closestPorts = portInfos.OrderBy(p => p.DistanceKm).Take(limit).ToList();

                return closestPorts;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving port distance and time from the ship: {ex.Message}");
            }
        }
    }
}
