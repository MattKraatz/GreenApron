using Xunit;

// Add all Tests to a single collection by default to suppress parallel testing
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]
namespace Tests.WebAPI
{
    public class Main
    {
        [Fact]
        public void TestingFrameworkLoads()
        {
            Assert.True(true);
        }
    }
}
