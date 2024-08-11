using AE_Backend.db;
using AE_Backend.General;
using AE_Backend.Model;
using AE_Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE_Backend.Tests.Services
{
    public class ShipServicesTests
    {
        private readonly Mock<MyDbContext> _dbContextMock = new Mock<MyDbContext>();
        //private readonly MyDbContext _dbContext;
        private readonly ShipServices _ShipServices;
        private readonly Utility _utility = new Utility();

        public ShipServicesTests()
        {
            _ShipServices = new ShipServices(_dbContextMock.Object, _utility);
        }

        #region InsertShip

        [Fact]
        public void InsertShip_Should_Handle_Exception()
        {
            var ShipDto = new ShipCreateParam
            {
                // Initialize properties as needed
            };

            _dbContextMock.Setup(db => db.Ships)
                          .Throws(new Exception("Database error"));

            var ShipServices = new ShipServices(_dbContextMock.Object, _utility);

            Assert.Throws<Exception>(() => ShipServices.InsertShip(ShipDto));
        }

        [Fact]
        public void InsertShip_Should_Handle_Empty_Properties()
        {
            var ShipDto = new ShipCreateParam
            {
                ShipName = null,
                Longitude = (decimal)-87.6298,
                Latitude = (decimal)41.8781,
                Velocity = (decimal)13.5,
                CreatedBy = "systest"
            };

            var ShipServices = new ShipServices(_dbContextMock.Object, _utility);

            Assert.Throws<Exception>(() => ShipServices.InsertShip(ShipDto));
        }

        #endregion

        #region GetAllShips:
        [Fact]
        public async Task GetAllShips_Should_Return_All_Active_Ships()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.Ships.Add(new Ship { ShipId = 1, ShipName = "Ship1", Longitude = (decimal)-87.6298, Latitude = (decimal)41.8781, Velocity = (decimal)13.5, RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.Ships.Add(new Ship { ShipId = 2, ShipName = "Ship2", Longitude = (decimal)151.2093, Latitude = (decimal)-33.8688, Velocity = (decimal)10.6, RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.Ships.Add(new Ship { ShipId = 3, ShipName = "Ship3", Longitude = (decimal)2.3522, Latitude = (decimal)48.8566, Velocity = (decimal)17.9, RowStatus = 0, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var ShipServices = new ShipServices(context, _utility);

                var result = await ShipServices.GetAllShips();

                Assert.Equal(2, result.Count());
                Assert.Contains(result, u => u.ShipName == "Ship1");
                Assert.Contains(result, u => u.ShipName == "Ship2");
            }
        }

        [Fact]
        public async Task GetAllShips_Should_Return_Empty_List_When_No_Ships()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDb")
                .Options;

            using (var context = new MyDbContext(options))
            {

            }

            using (var context = new MyDbContext(options))
            {
                var ShipServices = new ShipServices(context, _utility);
                var result = await ShipServices.GetAllShips();

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task GetAllShips_Should_Return_Only_Active_Ships()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "ActiveShipsDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.Ships.Add(new Ship { ShipId = 1, ShipName = "Ship1", Longitude = (decimal)-87.6298, Latitude = (decimal)41.8781, Velocity = (decimal)13.5, RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.Ships.Add(new Ship { ShipId = 2, ShipName = "Ship2", Longitude = (decimal)151.2093, Latitude = (decimal)-33.8688, Velocity = (decimal)10.6, RowStatus = 0, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var ShipServices = new ShipServices(context, _utility);

                var result = await ShipServices.GetAllShips();

                Assert.Single(result);
                Assert.Equal("Ship1", result.First().ShipName);
            }
        }

        [Fact]
        public void GetAllShips_Should_Perform_Well_For_Large_Data()
        {
            var largeShipCount = 10000;

            var ShipsDbSetMock = new Mock<DbSet<Ship>>();
            ShipsDbSetMock.As<IQueryable<Ship>>().Setup(u => u.GetEnumerator()).Returns(GenerateLargeShipList(largeShipCount).GetEnumerator());

            _dbContextMock.Setup(db => db.Ships).Returns(ShipsDbSetMock.Object);

            var ShipServices = new ShipServices(_dbContextMock.Object, _utility);

            var stopwatch = Stopwatch.StartNew();
            var result = ShipServices.GetAllShips();
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds < 1000);
        }

        #endregion

        #region GetShipById
        [Fact]
        public async Task GetShipById_Should_Return_Ship_When_Found()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "ShipDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.Ships.Add(new Ship { ShipId = 42, ShipName = "testShip", Longitude = (decimal)-87.6298, Latitude = (decimal)41.8781, Velocity = (decimal)13.5, RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var ShipServices = new ShipServices(context, _utility);

                var result = await ShipServices.GetShipById(42);

                Assert.NotNull(result);
                Assert.Equal("testShip", result.ShipName);
            }
        }

        #endregion

        #region UpdateShip

        [Fact]
        public void UpdateShip_Should_Throw_Exception_On_Invalid_Input()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var dbContext = new MyDbContext(options))
            {
                var ShipServices = new ShipServices(dbContext, _utility);
                var invalidShipDto = new ShipUpdateParam
                {
                };

                Assert.Throws<Exception>(() => ShipServices.UpdateShip(invalidShipDto));
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
        private List<Ship> GenerateLargeShipList(int count)
        {
            // Generate a large list of mock Ships
            // You can randomize properties or create a pattern based on your needs
            var ShipList = new List<Ship>();
            for (int i = 1; i <= count; i++)
            {
                ShipList.Add(new Ship { ShipId = i, ShipName = $"Ship{i}" });
            }
            return ShipList;
        }
    }
}
