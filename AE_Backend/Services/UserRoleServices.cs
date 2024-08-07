using AE_Backend.db;
using AE_Backend.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AE_Backend.Services
{
    public interface IUserRoleService
    {
        Task<int> InsertUserRole(UserRoleCreateParam userDto);
        Task<IEnumerable<UserRole>> GetAllUserRoles();
        Task<UserRole> GetUserRoleById(int userId);
        Task<UserRole> UpdateUserRole(UserRoleUpdateParam userDto);
        Task<string> DeleteUserRole(int userRoleId, string modifiedBy);
    }
    public class UserRoleServices : IUserRoleService
    {
        private readonly MyDbContext _dbContext;

        public UserRoleServices(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> InsertUserRole(UserRoleCreateParam userRoleDto)
        {
            try
            {
                var result = _dbContext.UserRoles
                .FromSqlRaw("EXEC [dbo].[SP_InsertUserRole] @userid, @roleid, @createdby",
                    new SqlParameter("@userid", userRoleDto.UserId),
                    new SqlParameter("@roleid", userRoleDto.RoleId),
                    new SqlParameter("@createdby", userRoleDto.CreatedBy))
                .AsEnumerable()
                .Select(u => u.UserRoleId)
                .FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting user role: {ex.Message}");
            }

        }

        public async Task<IEnumerable<UserRole>> GetAllUserRoles()
        {
            try
            {
                return await _dbContext.UserRoles
                    .Where(r => r.RowStatus == 1)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user roles: {ex.Message}");
            }
        }

        public async Task<UserRole> GetUserRoleById(int userRoleId)
        {
            try
            {
                return await _dbContext.UserRoles
                    .Where(u => u.UserRoleId == userRoleId && u.RowStatus == 1)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user role with ID {userRoleId}: {ex.Message}");
            }
        }


        public async Task<UserRole> UpdateUserRole(UserRoleUpdateParam useRolerDto)
        {
            try
            {
                var result = _dbContext.UserRoles
                .FromSqlRaw("EXEC [dbo].[SP_UpdateUserRole] @userroleid, @userid, @roleid, @isActive, @modifiedby",
                    new SqlParameter("@userroleid", useRolerDto.UserRoleId),
                    new SqlParameter("@userid", useRolerDto.UserId),
                    new SqlParameter("@roleid", useRolerDto.RoleId),
                    new SqlParameter("@isActive", useRolerDto.RowStatus),
                    new SqlParameter("@modifiedby", useRolerDto.ModifiedBy))
                .AsEnumerable()
                .FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error update user role: {ex.Message}");
            }
        }

        public async Task<string> DeleteUserRole(int userRoleId, string modifiedBy)
        {
            try
            {
                var result = await _dbContext.Database
                    .ExecuteSqlRawAsync("EXEC [dbo].[SP_DeleteUserRole] @userroleid, @modifiedby",
                        new SqlParameter("@userroleid", userRoleId),
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
                throw new Exception($"Error deleting user role: {ex.Message}");
            }
        }
    }
}
