using AE_Backend.db;
using AE_Backend.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AE_Backend.Services
{
    public interface IRoleService
    {
        Task<int> InsertRole(RoleCreateParam RoleDto);
        Task<IEnumerable<Role>> GetAllRoles();
        Task<Role?> GetRoleById(int RoleId);
        Role UpdateRole(RoleUpdateParam RoleDto);
        Task<string> DeleteRole(int roleId, string modifiedBy);
    }

    public class RoleServices : IRoleService
    {
        private readonly MyDbContext _dbContext;

        public RoleServices(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> InsertRole(RoleCreateParam roleDto)
        {
            try
            {
                bool roleNameExists = await _dbContext.Roles.AnyAsync(r => r.RoleName == roleDto.RoleName);
                if (roleNameExists)
                {
                    throw new Exception($"Role name '{roleDto.RoleName}' already exists.");
                }

                var result = _dbContext.Roles
                .FromSqlRaw("EXEC [dbo].[SP_InsertRole] @rolename, @createdby",
                    new SqlParameter("@rolename", roleDto.RoleName),
                    new SqlParameter("@createdby", roleDto.CreatedBy))
                .AsEnumerable()
                .Select(u => u.RoleId)
                .FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting user: {ex.Message}");
            }

        }

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            try
            {
                return await _dbContext.Roles.Where(r => r.RowStatus == 1).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving roles: {ex.Message}");
            }
        }

        public async Task<Role?> GetRoleById(int roleId)
        {
            try
            {
                return await _dbContext.Roles
                    .Where(r => r.RoleId == roleId && r.RowStatus == 1)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving role with ID {roleId}: {ex.Message}");
            }
        }


        public Role UpdateRole(RoleUpdateParam roleDto)
        {
            try
            {
                var result = _dbContext.Roles
                .FromSqlRaw("EXEC [dbo].[SP_UpdateRole] @roleid, @rolename, @isActive, @modifiedby",
                    new SqlParameter("@roleid", roleDto.RoleId),
                    new SqlParameter("@rolename", roleDto.RoleName),
                    new SqlParameter("@isActive", roleDto.RowStatus),
                    new SqlParameter("@modifiedby", roleDto.ModifiedBy))
                .AsEnumerable()
                .FirstOrDefault();

                if(result == null)
                {
                    throw new Exception("Role not found.");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error update role: {ex.Message}");
            }
        }

        public async Task<string> DeleteRole(int roleId, string modifiedBy)
        {
            try
            {
                var result = await _dbContext.Database
                    .ExecuteSqlRawAsync("EXEC [dbo].[SP_DeleteRole] @roleid, @modifiedby",
                        new SqlParameter("@roleid", roleId),
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
                throw new Exception($"Error deleting role: {ex.Message}");
            }
        }
    }
}
