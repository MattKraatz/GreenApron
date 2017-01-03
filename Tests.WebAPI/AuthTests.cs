using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using WebAPI;
using System.Threading.Tasks;

namespace Tests.WebAPI
{
    public class AuthTests
    {
        private TaskManager _task { get; set; } = new TaskManager();

        public AuthTests()
        {
            
        }

        [Fact]
        public async void CanRegisterUser()
        {
            AuthResponse response = await _task.RegisterUser();
            Assert.NotNull(response);
            Assert.True(response.success);
            Assert.NotNull(response.user);
            await _task.DeleteUser();
        }

        [Fact]
        public async void CannotRegisterDuplicateUsername()
        {
            await _task.RegisterUser();
            JsonResponse response = await _task.RegisterUser();
            Assert.NotNull(response);
            Assert.False(response.success);
            await _task.DeleteUser();
        }

        [Fact]
        public async void CanLoginUser()
        {
            await _task.RegisterUser();
            AuthResponse response = await _task.LoginUser(true);
            Assert.NotNull(response);
            Assert.True(response.success);
            Assert.NotNull(response.user);
            await _task.DeleteUser();
        }

        [Fact]
        public async void CannotLoginUserWithIncorrectPassword()
        {
            await _task.RegisterUser();
            JsonResponse response = await _task.LoginUser(false);
            Assert.NotNull(response);
            Assert.False(response.success);
            await _task.DeleteUser();
        }

        [Fact]
        public async void CanDeleteUser()
        {
            await _task.RegisterUser();
            JsonResponse response = await _task.DeleteUser();
            Assert.NotNull(response);
            Assert.True(response.success);
        }
    }
}
