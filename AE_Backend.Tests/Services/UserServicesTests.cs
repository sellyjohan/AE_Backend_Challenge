using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using AE_Backend.Model;
using AE_Backend.Services;
using AE_Backend.db;
using Microsoft.VisualBasic;
using System.Diagnostics;
using AE_Backend.General;

namespace AE_Backend.Tests.Services
{
    public class UserServicesTests
    {
        private readonly Mock<MyDbContext> _dbContextMock = new Mock<MyDbContext>();
        //private readonly MyDbContext _dbContext;
        private readonly UserServices _userServices;

        public UserServicesTests()
        {
            _userServices = new UserServices(_dbContextMock.Object);
        }

        #region InsertUser

        [Fact]
        public void InsertUser_Should_Handle_Exception()
        {
            var userDto = new UserCreateParam
            {
                // Initialize properties as needed
            };

            _dbContextMock.Setup(db => db.Users)
                          .Throws(new Exception("Database error"));

            var userServices = new UserServices(_dbContextMock.Object);

            Assert.Throws<Exception>(() => userServices.InsertUser(userDto));
        }

        [Fact]
        public void InsertUser_Should_Handle_Empty_Properties()
        {
            var userDto = new UserCreateParam
            {
                Username = "unittest",
                Fullname = null,
                Birthdate = DateAndTime.Now,
                CreatedBy = "systest"
            };

            var userServices = new UserServices(_dbContextMock.Object);

            Assert.Throws<Exception>(() => userServices.InsertUser(userDto));
        }

        #endregion

        #region GetAllUsers:
        [Fact]
        public async Task GetAllUsers_Should_Return_All_Active_Users()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.Users.Add(new User { UserId = 1, Username = "user1", Fullname = "user 1", Birthdate = new DateTime(1990, 1, 1), RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.Users.Add(new User { UserId = 2, Username = "user2", Fullname = "user 2", Birthdate = new DateTime(1990, 1, 1), RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.Users.Add(new User { UserId = 3, Username = "user3", Fullname = "user 3", Birthdate = new DateTime(1990, 1, 1), RowStatus = 0, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var userServices = new UserServices(context);
                
                var result = await userServices.GetAllUsers();
                
                Assert.Equal(5, result.Count());
                Assert.Contains(result, u => u.Username == "user1");
                Assert.Contains(result, u => u.Username == "user2");
            }
        }

        [Fact]
        public async Task GetAllUsers_Should_Return_Empty_List_When_No_Users()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDb")
                .Options;

            using (var context = new MyDbContext(options))
            {

            }

            using (var context = new MyDbContext(options))
            {
                var userServices = new UserServices(context);
                var result = await userServices.GetAllUsers();

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task GetAllUsers_Should_Return_Only_Active_Users()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "ActiveUsersDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.Users.Add(new User { UserId = 1, Username = "user1", Fullname = "user 1", Birthdate = new DateTime(1990, 1, 1), RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.Users.Add(new User { UserId = 2, Username = "user2", Fullname = "user 2", Birthdate = new DateTime(1990, 1, 1), RowStatus = 0, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var userServices = new UserServices(context);
                
                var result = await userServices.GetAllUsers();

                Assert.Single(result);
                Assert.Equal("user1", result.First().Username);
            }
        }

        [Fact]
        public void GetAllUsers_Should_Perform_Well_For_Large_Data()
        {
            var largeUserCount = 10000;

            var usersDbSetMock = new Mock<DbSet<User>>();
            usersDbSetMock.As<IQueryable<User>>().Setup(u => u.GetEnumerator()).Returns(GenerateLargeUserList(largeUserCount).GetEnumerator());

            _dbContextMock.Setup(db => db.Users).Returns(usersDbSetMock.Object);

            var userServices = new UserServices(_dbContextMock.Object);

            var stopwatch = Stopwatch.StartNew();
            var result = userServices.GetAllUsers();
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds < 1000);
        }

        #endregion

        #region GetUserById
        [Fact]
        public async Task GetUserById_Should_Return_User_When_Found()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "UserDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.Users.Add(new User { UserId = 42, Username = "testuser", Fullname = "user 1", Birthdate = new DateTime(1990, 1, 1), RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var userServices = new UserServices(context);

                var result = await userServices.GetUserById(42);

                Assert.NotNull(result);
                Assert.Equal("testuser", result.Username);
            }
        }

        #endregion

        #region UpdateUser

        [Fact]
        public void UpdateUser_Should_Throw_Exception_On_Invalid_Input()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var dbContext = new MyDbContext(options))
            {
                var userServices = new UserServices(dbContext);
                var invalidUserDto = new UserUpdateParam
                {
                };

                Assert.Throws<Exception>(() => userServices.UpdateUser(invalidUserDto));
            }
        }
        #endregion

        #region GetShipsForUser
        [Fact]
        public async Task GetShipsForUser_Should_Return_Ships_For_Existing_User()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var dbContext = new MyDbContext(options))
            {
                var userId = 1;
                var InsertShips = new List<Ship>
                {
                    new Ship { ShipId = 101, ShipName = "ship1", RowStatus = 1, CreatedBy = "admin", ModifiedBy = "admin" },
                    new Ship { ShipId = 102, ShipName = "ship1", RowStatus = 1, CreatedBy = "admin", ModifiedBy = "admin" }
                };
                dbContext.Ships.AddRange(InsertShips);
                await dbContext.SaveChangesAsync();

                var userShips = new List<UserShip>
                {
                    new UserShip { UserId = userId, ShipId = 101, RowStatus = 1, CreatedBy = "admin", ModifiedBy = "admin" },
                    new UserShip { UserId = userId, ShipId = 102, RowStatus = 1, CreatedBy = "admin", ModifiedBy = "admin" }
                };
                dbContext.UserShips.AddRange(userShips);
                await dbContext.SaveChangesAsync();

                var userServices = new UserServices(dbContext);

                var ships = await userServices.GetShipsForUser(userId);

                Assert.NotNull(ships);
                Assert.Equal(2, ships.Count());
            }
        }

        [Fact]
        public async Task GetShipsForUser_Should_Return_Empty_List_For_NonExisting_User()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var dbContext = new MyDbContext(options))
            {
                var nonExistingUserId = 999;
                var userServices = new UserServices(dbContext);

                var ships = await userServices.GetShipsForUser(nonExistingUserId);

                Assert.Empty(ships);
            }
        }
        #endregion

        private DbSet<T> MockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
            return mockDbSet.Object;
        }
        private List<User> GenerateLargeUserList(int count)
        {
            // Generate a large list of mock users
            // You can randomize properties or create a pattern based on your needs
            var userList = new List<User>();
            for (int i = 1; i <= count; i++)
            {
                userList.Add(new User { UserId = i, Username = $"user{i}" });
            }
            return userList;
        }
    }
}

