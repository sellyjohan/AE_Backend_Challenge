using AE_Backend.db;
using AE_Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using AE_Backend.General;

namespace AE_Backend.Services
{
    public interface IPortService
    {
        Task<IEnumerable<Port>> GetAllPorts();
        Task<Port?> GetPortById(int portId);
    }
    public class PortServices : IPortService
    {
        private readonly MyDbContext _dbContext;

        public PortServices(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Port>> GetAllPorts()
        {
            try
            {
                return await _dbContext.Ports
                    .Where(r => r.RowStatus == 1)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving Ports: {ex.Message}");
            }
        }

        public async Task<Port?> GetPortById(int portId)
        {
            try
            {
                return await _dbContext.Ports
                    .Where(u => u.PortId == portId && u.RowStatus == 1)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving port with ID {portId}: {ex.Message}");
            }
        }
    }
}
