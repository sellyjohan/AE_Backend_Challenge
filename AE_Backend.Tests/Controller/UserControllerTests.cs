using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AE_Backend.Controllers;
using AE_Backend.db;
using AE_Backend.Model;
using AE_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace AE_Backend.Tests.Controller
{
    public class UserControllerTests
    {
        //private readonly MyDbContext _context;
        [Fact]
        public async Task GetAllUsers_Should_Return_Users()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var dbContext = new MyDbContext(options))
            {
                var expectedUsers = new List<User>
                {
                    new User { UserId = 4, Username = "user4", Fullname = "user 4", Birthdate = new DateTime(1990, 1, 1), RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" },
                    new User { UserId = 5, Username = "user5", Fullname = "user 5", Birthdate = new DateTime(1990, 1, 1), RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" },
                    new User { UserId = 6, Username = "user6", Fullname = "user 6", Birthdate = new DateTime(1990, 1, 1), RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" }
                };
                dbContext.Users.AddRange(expectedUsers);
                await dbContext.SaveChangesAsync();

                var controller = new UsersController(new UserServices(dbContext));

                var result = await controller.GetAllUsers();

                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var actualUsers = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
                Assert.Equal(expectedUsers, actualUsers);
            }
        }

    }
}
