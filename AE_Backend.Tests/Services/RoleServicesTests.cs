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
    public class RoleServicesTests
    {
        private readonly Mock<MyDbContext> _dbContextMock = new Mock<MyDbContext>();
        //private readonly MyDbContext _dbContext;
        private readonly RoleServices _roleServices;

        public RoleServicesTests()
        {
            _roleServices = new RoleServices(_dbContextMock.Object);
        }

        #region InsertRole

        [Fact]
        public void InsertRole_Should_Handle_Exception()
        {
            var RoleDto = new RoleCreateParam
            {
                // Initialize properties as needed
            };

            _dbContextMock.Setup(db => db.Roles)
                          .Throws(new Exception("Database error"));

            var RoleServices = new RoleServices(_dbContextMock.Object);

            Assert.Throws<Exception>(() => RoleServices.InsertRole(RoleDto));
        }

        [Fact]
        public void InsertRole_Should_Handle_Empty_Properties()
        {
            var RoleDto = new RoleCreateParam
            {
                RoleName = "unittest",
                CreatedBy = "systest"
            };

            var RoleServices = new RoleServices(_dbContextMock.Object);

            Assert.Throws<Exception>(() => RoleServices.InsertRole(RoleDto));
        }

        #endregion

        #region GetAllRoles:
        [Fact]
        public async Task GetAllRoles_Should_Return_All_Active_Roles()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.Roles.Add(new Role { RoleId = 1, RoleName = "Role1", RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.Roles.Add(new Role { RoleId = 2, RoleName = "Role2", RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.Roles.Add(new Role { RoleId = 3, RoleName = "Role3", RowStatus = 0, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var RoleServices = new RoleServices(context);

                var result = await RoleServices.GetAllRoles();

                Assert.Equal(2, result.Count());
                Assert.Contains(result, u => u.RoleName == "Role1");
                Assert.Contains(result, u => u.RoleName == "Role2");
            }
        }

        [Fact]
        public async Task GetAllRoles_Should_Return_Empty_List_When_No_Roles()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDb")
                .Options;

            using (var context = new MyDbContext(options))
            {

            }

            using (var context = new MyDbContext(options))
            {
                var RoleServices = new RoleServices(context);
                var result = await RoleServices.GetAllRoles();

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task GetAllRoles_Should_Return_Only_Active_Roles()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "ActiveRolesDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.Roles.Add(new Role { RoleId = 1, RoleName = "Role1", RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.Roles.Add(new Role { RoleId = 2, RoleName = "Role2", RowStatus = 0, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var RoleServices = new RoleServices(context);

                var result = await RoleServices.GetAllRoles();

                Assert.Single(result);
                Assert.Equal("Role1", result.First().RoleName);
            }
        }

        [Fact]
        public void GetAllRoles_Should_Perform_Well_For_Large_Data()
        {
            var largeRoleCount = 10000;

            var RolesDbSetMock = new Mock<DbSet<Role>>();
            RolesDbSetMock.As<IQueryable<Role>>().Setup(u => u.GetEnumerator()).Returns(GenerateLargeRoleList(largeRoleCount).GetEnumerator());

            _dbContextMock.Setup(db => db.Roles).Returns(RolesDbSetMock.Object);

            var RoleServices = new RoleServices(_dbContextMock.Object);

            var stopwatch = Stopwatch.StartNew();
            var result = RoleServices.GetAllRoles();
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds < 1000);
        }

        #endregion

        #region GetRoleById
        [Fact]
        public async Task GetRoleById_Should_Return_Role_When_Found()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "RoleDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.Roles.Add(new Role { RoleId = 42, RoleName = "testRole", RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var RoleServices = new RoleServices(context);

                var result = await RoleServices.GetRoleById(42);

                Assert.NotNull(result);
                Assert.Equal("testRole", result.RoleName);
            }
        }

        #endregion

        #region UpdateRole

        [Fact]
        public void UpdateRole_Should_Throw_Exception_On_Invalid_Input()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var dbContext = new MyDbContext(options))
            {
                var RoleServices = new RoleServices(dbContext);
                var invalidRoleDto = new RoleUpdateParam
                {
                };

                Assert.Throws<Exception>(() => RoleServices.UpdateRole(invalidRoleDto));
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
        private List<Role> GenerateLargeRoleList(int count)
        {
            // Generate a large list of mock Roles
            // You can randomize properties or create a pattern based on your needs
            var RoleList = new List<Role>();
            for (int i = 1; i <= count; i++)
            {
                RoleList.Add(new Role { RoleId = i, RoleName = $"Role{i}" });
            }
            return RoleList;
        }
    }
}
