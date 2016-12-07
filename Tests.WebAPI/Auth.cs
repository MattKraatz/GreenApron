using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using WebAPI;
using Microsoft.AspNetCore.Mvc;

namespace Tests.WebAPI
{
    public class AuthTests
    {
        var auth = new Auth();

        [Fact]
        public void CanRegisterNewUser()
        {
            var creds = new RegistrationCredentials() { username = "MattKraatz", password = "Password1", confirmPassword = "Password1" };
            IActionResult response = auth.Register(creds);
            Assert.NotNull(response);
        }

        [Fact]
        public void CanLoginUser()
        {
            var creds = new LoginCredentials() { username = "MattKraatz", password = "Password1" };
            IActionResult response = auth.Login(creds);
            Assert.NotNull(response);
        }
    }
}
