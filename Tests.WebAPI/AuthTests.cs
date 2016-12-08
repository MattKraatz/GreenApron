using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Data;
using WebAPI.Controllers;
using WebAPI.Models;

namespace Tests.WebAPI
{
    public class AuthTests
    {
        static ApplicationDbContext _context;
        public AuthTests()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connection = Environment.GetEnvironmentVariable("GreenApronDB");
            builder.UseSqlServer(connection)
                    .UseInternalServiceProvider(serviceProvider);
            _context = new ApplicationDbContext(builder.Options);
            _context.Database.Migrate();
        }

        Guid userId = new Guid();

        [Fact]
        public void CanRegisterNewUser()
        {

        }

        [Fact]
        public void CanLoginUser()
        {

        }

        [Fact]
        public void CanDeleteUser()
        {

        }
    }
}
