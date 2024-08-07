using AE_Backend.db;
using AE_Backend.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AE_Backend.Services
{
    public interface IUserShipService
    {
        Task<int> InsertUserShip(UserShipCreateParam shipDto);
        Task<IEnumerable<UserShip>> GetAllUserShips();
        Task<UserShip> GetUserShipById(int shipId);
        Task<UserShip> UpdateUserShip(UserShipUpdateParam shipDto);
        Task<string> DeleteUserShip(int shipId, string ModifiedBy);
    }
    public class UserShipServices : IUserShipService
    {
        private readonly MyDbContext _dbContext;

        public UserShipServices(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> InsertUserShip(UserShipCreateParam userShipDto)
        {
            try
            {
                var result = _dbContext.UserShips
                .FromSqlRaw("EXEC [dbo].[SP_InsertUserShip] @userid, @shipid, @createdby",
                    new SqlParameter("@userid", userShipDto.UserId),
                    new SqlParameter("@shipid", userShipDto.ShipId),
                    new SqlParameter("@createdby", userShipDto.CreatedBy))
                .AsEnumerable()
                .Select(u => u.UserShipId)
                .FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting usership: {ex.Message}");
            }
        }

        public async Task<IEnumerable<UserShip>> GetAllUserShips()
        {
            try
            {
                return await _dbContext.UserShips
                    .Where(r => r.RowStatus == 1)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving userships: {ex.Message}");
            }
        }

        public async Task<UserShip> GetUserShipById(int usershipId)
        {
            try
            {
                return await _dbContext.UserShips
                    .Where(u => u.UserShipId == usershipId && u.RowStatus == 1)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving usership with ID {usershipId}: {ex.Message}");
            }
        }

        public async Task<UserShip> UpdateUserShip(UserShipUpdateParam userShipDto)
        {
            try
            {
                var result = _dbContext.UserShips
                .FromSqlRaw("EXEC [dbo].[SP_UpdateUserShip] @usershipid, @userid, @shipid, @isActive, @modifiedby",
                    new SqlParameter("@usershipid", userShipDto.UserShipId),
                    new SqlParameter("@userid", userShipDto.UserId),
                    new SqlParameter("@shipid", userShipDto.ShipId),
                    new SqlParameter("@isActive", userShipDto.RowStatus),
                    new SqlParameter("@modifiedby", userShipDto.ModifiedBy))
                .AsEnumerable()
                .FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error update usership: {ex.Message}");
            }
        }

        public async Task<string> DeleteUserShip(int UserShipId, string ModifiedBy)
        {
            try
            {
                var result = await _dbContext.Database
                    .ExecuteSqlRawAsync("EXEC [dbo].[SP_DeleteUserShip] @userid, @modifiedby",
                        new SqlParameter("@userid", UserShipId),
                        new SqlParameter("@modifiedby", ModifiedBy));

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
                throw new Exception($"Error deleting usership: {ex.Message}");
            }
        }
    }
}
