using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using WebAPI;
using System.Reflection;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Tests.WebAPI
{
    public class AuthTests
    {
        private GreenApronContext _context { get; set; }
        private AuthController _authCont { get; set; }
        private TaskFactory _taskFact { get; set; } = new TaskFactory();
        
        // Constructor for injecting context
        public AuthTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<GreenApronContext>();
            builder.UseSqlServer(Environment.GetEnvironmentVariable("GreenApronDB"))
                    .UseInternalServiceProvider(serviceProvider);
            _context = new GreenApronContext(builder.Options);

            // Instantiate an Auth Controller with database context
            _authCont = new AuthController(_context);
        }

        [Fact]
        public async void CanRegisterUser()
        {
            var user = new User() { Username = "TestUser192837", FirstName = "Test", LastName = "User", Password = "Password1"};
            var json = await _authCont.Register(user);
            JsonResponse response = (JsonResponse)json.Value;
            Assert.NotNull(response);
            Assert.True(response.success);
        }

        [Fact]
        public async void CannotRegisterDuplicateUsername()
        {
            var user = new User() { Username = "TestUser192837", FirstName = "Test", LastName = "User", Password = "Password1" };
            var json = await _authCont.Register(user);
            JsonResponse response = (JsonResponse)json.Value;
            Assert.NotNull(response);
            Assert.False(response.success);
        }

        [Fact]
        public async void CanLoginUser()
        {
            var user = new User() { Username = "TestUser192837", Password = "Password1" };
            var json = await _authCont.Login(user);
            JsonResponse response = (JsonResponse)json.Value;
            Assert.NotNull(response);
            Assert.True(response.success);
        }

        [Fact]
        public async void CannotLoginUserWithIncorrectPassword()
        {
            var user = new User() { Username = "TestUser192837", Password = "Password2" };
            var json = await _authCont.Login(user);
            JsonResponse response = (JsonResponse)json.Value;
            Assert.NotNull(response);
            Assert.False(response.success);
        }

        [Fact]
        public async void CanDeleteUser()
        {
            var json = await _authCont.Delete("TestUser192837");
            JsonResponse response = (JsonResponse)json.Value;
            Assert.NotNull(response);
            Assert.True(response.success);
        }
    }
}
