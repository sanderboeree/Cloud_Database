using System;
using System.Net.Http;
using Xunit;

namespace OnlineStore.Api.Integration.Tests
{
    public class UnitTestTVPS3
    {
        HttpClient Client { get; }
        public UnitTestTVPS3()
        {
            Client = new HttpClient();
            // Client.BaseAddress = new Uri($"http://##linktoUnitTestsDeployment##:{5001}");
            Client.BaseAddress = new Uri($"http://localhost:{5001}");
        }
        [Fact]
        public void AlwaysPass() {
            Assert.True(true);
        }

        // [Theory]
        // public void LogIn()
        // {
        //     Assert.True(Client.PostAsync(Client.BaseAddress + "/v1/Authentication/Login"));
        // }
        
    }
}