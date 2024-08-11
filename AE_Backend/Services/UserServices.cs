using AE_Backend.db;
using AE_Backend.General;
using AE_Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AE_Backend.Services
{
    public interface IUserService
    {
        int InsertUser(UserCreateParam userDto);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int userId);
        User UpdateUser(UserUpdateParam userDto);
        Task<string> DeleteUser(int userId, string ModifiedBy);
        Task<IEnumerable<Ship?>> GetShipsForUser(int userId);
    }

    public class UserServices : IUserService
    {
        private readonly MyDbContext _dbContext;

        public UserServices(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int InsertUser(UserCreateParam userDto)
        {
            try
            {
                var result = _dbContext.Users
                .FromSqlRaw("EXEC [dbo].[SP_InsertUser] @username, @fullname, @birthdate, @createdby",
                    new SqlParameter("@username", userDto.Username),
                    new SqlParameter("@fullname", userDto.Fullname),
                    new SqlParameter("@birthdate", userDto.Birthdate),
                    new SqlParameter("@createdby", userDto.CreatedBy))
                .AsEnumerable()
                .Select(u => u.UserId)
                .FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting user: {ex.Message}");
            }
            
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .Include(us => us.UserShips)
                        .ThenInclude(s => s.Ship)
                    .Where(r => r.RowStatus == 1)
                .ToListAsync();

                //if (user == null || user.Count == 0)
                //{
                //    throw new CustomException.UserNotFoundException($"No active user found in the database.");
                //}

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving users: {ex.Message}");
            }
        }

        public async Task<User> GetUserById(int userId)
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .Include(us => us.UserShips)
                        .ThenInclude(s => s.Ship)
                    .Where(u => u.UserId == userId && u.RowStatus == 1)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new CustomException.UserNotFoundException($"User with ID {userId} not found.");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving user with ID {userId}: {ex.Message}");
            }
        }

        public User UpdateUser(UserUpdateParam userDto)
        {
            try
            {
                if (userDto.UserId <= 0 || string.IsNullOrWhiteSpace(userDto.Username))
                {
                    throw new ArgumentException("Invalid input parameters for user update.");
                }

                var result = _dbContext.Users
                .FromSqlRaw("EXEC [dbo].[SP_UpdateUser] @userid, @username, @fullname, @birthdate, @isActive, @modifiedby",
                    new SqlParameter("@userid", userDto.UserId),
                    new SqlParameter("@username", userDto.Username),
                    new SqlParameter("@fullname", userDto.Fullname),
                    new SqlParameter("@birthdate", userDto.Birthdate),
                    new SqlParameter("@isActive", userDto.RowStatus),
                    new SqlParameter("@modifiedby", userDto.ModifiedBy))
                .AsEnumerable()
                .FirstOrDefault();

                if (result == null)
                {
                    throw new DbUpdateException("Error updating user: Result is null.");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error update user: {ex.Message}");
            }
        }

        public async Task<string> DeleteUser(int userId, string modifiedBy)
        {
            try
            {
                var result = await _dbContext.Database
                    .ExecuteSqlRawAsync("EXEC [dbo].[SP_DeleteUser] @userid, @modifiedby",
                        new SqlParameter("@userid", userId),
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
                throw new Exception($"Error deleting user: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Ship?>> GetShipsForUser(int userId)
        {
            try
            {
                var result = await _dbContext.UserShips
                                .Where(us => us.UserId == userId && us.RowStatus == 1)
                                .Include(us => us.Ship)
                                .Select(us => us.Ship)
                                .ToListAsync();

                if (result == null)
                {
                    throw new CustomException.ShipNotFoundException($"Ship assigned to user ID {userId} not found.");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving ship information of the user with ID {userId}: {ex.Message}");
            }
        }
    }
}
