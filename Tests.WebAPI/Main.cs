using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.WebAPI
{
    public class Main
    {
        [Fact]
        public void FrameworkInstantiates()
        {
            Assert.Null(null);
            Assert.True(true);
            Assert.NotNull(1);
        }
    }
}
