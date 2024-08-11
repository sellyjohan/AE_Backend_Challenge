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
    public class UserShipServicesTests
    {
        private readonly Mock<MyDbContext> _dbContextMock = new Mock<MyDbContext>();
        //private readonly MyDbContext _dbContext;
        private readonly UserShipServices _UserShipServices;

        public UserShipServicesTests()
        {
            _UserShipServices = new UserShipServices(_dbContextMock.Object);
        }

        #region InsertUserShip

        [Fact]
        public void InsertUserShip_Should_Handle_Exception()
        {
            var UserShipDto = new UserShipCreateParam
            {
                // Initialize properties as needed
            };

            _dbContextMock.Setup(db => db.UserShips)
                          .Throws(new Exception("Database error"));

            var UserShipServices = new UserShipServices(_dbContextMock.Object);

            Assert.Throws<Exception>(() => UserShipServices.InsertUserShip(UserShipDto));
        }

        #endregion

        #region GetAllUserShips:
        [Fact]
        public async Task GetAllUserShips_Should_Return_All_Active_UserShips()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.UserShips.Add(new UserShip { UserShipId = 1, UserId = 1, ShipId = 1, RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.UserShips.Add(new UserShip { UserShipId = 2, UserId = 2, ShipId = 2, RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.UserShips.Add(new UserShip { UserShipId = 3, UserId = 3, ShipId = 3, RowStatus = 0, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var UserShipServices = new UserShipServices(context);

                var result = await UserShipServices.GetAllUserShips();

                Assert.Equal(2, result.Count());
                Assert.Contains(result, u => (u.UserId == 1 && u.ShipId == 1));
                Assert.Contains(result, u => (u.UserId == 2 && u.ShipId == 2));
            }
        }

        [Fact]
        public async Task GetAllUserShips_Should_Return_Empty_List_When_No_UserShips()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDb")
                .Options;

            using (var context = new MyDbContext(options))
            {

            }

            using (var context = new MyDbContext(options))
            {
                var UserShipServices = new UserShipServices(context);
                var result = await UserShipServices.GetAllUserShips();

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task GetAllUserShips_Should_Return_Only_Active_UserShips()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "ActiveUserShipsDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.UserShips.Add(new UserShip { UserShipId = 1, UserId = 1, ShipId = 1, RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.UserShips.Add(new UserShip { UserShipId = 2, UserId = 2, ShipId = 2, RowStatus = 0, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var UserShipServices = new UserShipServices(context);

                var result = await UserShipServices.GetAllUserShips();

                Assert.Single(result);
                Assert.Equal(1, result.First().UserId);
            }
        }

        [Fact]
        public void GetAllUserShips_Should_Perform_Well_For_Large_Data()
        {
            var largeUserShipCount = 10000;

            var UserShipsDbSetMock = new Mock<DbSet<UserShip>>();
            UserShipsDbSetMock.As<IQueryable<UserShip>>().Setup(u => u.GetEnumerator()).Returns(GenerateLargeUserShipList(largeUserShipCount).GetEnumerator());

            _dbContextMock.Setup(db => db.UserShips).Returns(UserShipsDbSetMock.Object);

            var UserShipServices = new UserShipServices(_dbContextMock.Object);

            var stopwatch = Stopwatch.StartNew();
            var result = UserShipServices.GetAllUserShips();
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds < 1000);
        }

        #endregion

        #region GetUserShipById
        [Fact]
        public async Task GetUserShipById_Should_Return_UserShip_When_Found()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "UserShipDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.UserShips.Add(new UserShip { UserShipId = 42, UserId = 4, ShipId = 4, RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var UserShipServices = new UserShipServices(context);

                var result = await UserShipServices.GetUserShipById(42);

                Assert.NotNull(result);
                Assert.Equal(4, result.UserId);
            }
        }

        #endregion

        #region UpdateUserShip

        [Fact]
        public void UpdateUserShip_Should_Throw_Exception_On_Invalid_Input()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var dbContext = new MyDbContext(options))
            {
                var UserShipServices = new UserShipServices(dbContext);
                var invalidUserShipDto = new UserShipUpdateParam
                {
                };

                Assert.Throws<Exception>(() => UserShipServices.UpdateUserShip(invalidUserShipDto));
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
        private List<UserShip> GenerateLargeUserShipList(int count)
        {
            // Generate a large list of mock UserShips
            // You can randomize properties or create a pattern based on your needs
            var UserShipList = new List<UserShip>();
            for (int i = 1; i <= count; i++)
            {
                UserShipList.Add(new UserShip { UserShipId = i, UserId = i, ShipId = i });
            }
            return UserShipList;
        }
    }
}
