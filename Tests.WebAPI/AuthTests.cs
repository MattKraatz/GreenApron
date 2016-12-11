using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace Tests.WebAPI
{
    public class AuthTests
    {
        private GreenApronContext _context { get; set; }

        public AuthTests()
        {
            var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkSqlServer()
            .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<GreenApronContext>();
            builder.UseSqlServer($"Server=(localdb)\\mssqllocaldb;Database=monsters_db_{Guid.NewGuid()};Trusted_Connection=True;MultipleActiveResultSets=true")
                    .UseInternalServiceProvider(serviceProvider);
            _context = new GreenApronContext(builder.Options);
            _context.Database.Migrate();
        }

        [Fact]
        public void CanRegisterUser()
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
