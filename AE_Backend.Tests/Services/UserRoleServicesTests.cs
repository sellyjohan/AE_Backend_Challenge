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
    public class UserRoleServicesTests
    {
        private readonly Mock<MyDbContext> _dbContextMock = new Mock<MyDbContext>();
        //private readonly MyDbContext _dbContext;
        private readonly UserRoleServices _UserRoleServices;

        public UserRoleServicesTests()
        {
            _UserRoleServices = new UserRoleServices(_dbContextMock.Object);
        }

        #region InsertUserRole

        [Fact]
        public void InsertUserRole_Should_Handle_Exception()
        {
            var UserRoleDto = new UserRoleCreateParam
            {
                // Initialize properties as needed
            };

            _dbContextMock.Setup(db => db.UserRoles)
                          .Throws(new Exception("Database error"));

            var UserRoleServices = new UserRoleServices(_dbContextMock.Object);

            Assert.Throws<Exception>(() => UserRoleServices.InsertUserRole(UserRoleDto));
        }

        #endregion

        #region GetAllUserRoles:
        [Fact]
        public async Task GetAllUserRoles_Should_Return_All_Active_UserRoles()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.UserRoles.Add(new UserRole { UserRoleId = 1, UserId = 1, RoleId = 1, RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.UserRoles.Add(new UserRole { UserRoleId = 2, UserId = 2, RoleId = 2, RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.UserRoles.Add(new UserRole { UserRoleId = 3, UserId = 3, RoleId = 3, RowStatus = 0, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var UserRoleServices = new UserRoleServices(context);

                var result = await UserRoleServices.GetAllUserRoles();

                Assert.Equal(2, result.Count());
                Assert.Contains(result, u => u.UserId == 1);
                Assert.Contains(result, u => u.UserId == 2);
            }
        }

        [Fact]
        public async Task GetAllUserRoles_Should_Return_Empty_List_When_No_UserRoles()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "EmptyDb")
                .Options;

            using (var context = new MyDbContext(options))
            {

            }

            using (var context = new MyDbContext(options))
            {
                var UserRoleServices = new UserRoleServices(context);
                var result = await UserRoleServices.GetAllUserRoles();

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task GetAllUserRoles_Should_Return_Only_Active_UserRoles()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "ActiveUserRolesDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.UserRoles.Add(new UserRole { UserRoleId = 1, UserId = 1, RoleId = 1, RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.UserRoles.Add(new UserRole { UserRoleId = 2, UserId = 2, RoleId = 2, RowStatus = 0, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var UserRoleServices = new UserRoleServices(context);

                var result = await UserRoleServices.GetAllUserRoles();

                Assert.Single(result);
                Assert.Equal(1, result.First().UserId);
            }
        }

        [Fact]
        public void GetAllUserRoles_Should_Perform_Well_For_Large_Data()
        {
            var largeUserRoleCount = 10000;

            var UserRolesDbSetMock = new Mock<DbSet<UserRole>>();
            UserRolesDbSetMock.As<IQueryable<UserRole>>().Setup(u => u.GetEnumerator()).Returns(GenerateLargeUserRoleList(largeUserRoleCount).GetEnumerator());

            _dbContextMock.Setup(db => db.UserRoles).Returns(UserRolesDbSetMock.Object);

            var UserRoleServices = new UserRoleServices(_dbContextMock.Object);

            var stopwatch = Stopwatch.StartNew();
            var result = UserRoleServices.GetAllUserRoles();
            stopwatch.Stop();

            Assert.True(stopwatch.ElapsedMilliseconds < 1000);
        }

        #endregion

        #region GetUserRoleById
        [Fact]
        public async Task GetUserRoleById_Should_Return_UserRole_When_Found()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "UserRoleDb")
                .Options;

            using (var context = new MyDbContext(options))
            {
                context.UserRoles.Add(new UserRole { UserRoleId = 42, UserId = 4, RoleId = 4, RowStatus = 1, CreatedBy = "systest", ModifiedBy = "systest" });
                context.SaveChanges();
            }

            using (var context = new MyDbContext(options))
            {
                var UserRoleServices = new UserRoleServices(context);

                var result = await UserRoleServices.GetUserRoleById(42);

                Assert.NotNull(result);
                Assert.Equal(4, result.UserId);
            }
        }

        #endregion

        #region UpdateUserRole

        [Fact]
        public void UpdateUserRole_Should_Throw_Exception_On_Invalid_Input()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var dbContext = new MyDbContext(options))
            {
                var UserRoleServices = new UserRoleServices(dbContext);
                var invalidUserRoleDto = new UserRoleUpdateParam
                {
                };

                Assert.Throws<Exception>(() => UserRoleServices.UpdateUserRole(invalidUserRoleDto));
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
        private List<UserRole> GenerateLargeUserRoleList(int count)
        {
            // Generate a large list of mock UserRoles
            // You can randomize properties or create a pattern based on your needs
            var UserRoleList = new List<UserRole>();
            for (int i = 1; i <= count; i++)
            {
                UserRoleList.Add(new UserRole { UserRoleId = i, UserId = i, RoleId = i });
            }
            return UserRoleList;
        }
    }
}
