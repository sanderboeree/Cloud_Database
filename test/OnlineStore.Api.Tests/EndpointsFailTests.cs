using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace OnlineStore.Api.Tests
{
    public class EndpointsFailTests
    {
        HttpClient Client { get; }

        public EndpointsFailTests()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri($"https://localhost:{5001}");
        }

        [Fact]
        public async Task FailAdmin()
        {
            var Response = await Client.GetAsync($"/v1/Admin");

            Assert.Equal(HttpStatusCode.BadRequest, Response.StatusCode);
        }

        [Fact]
        public async Task FailCoaches()
        {
            var Response = await Client.GetAsync($"/v1/Coaches");

            Assert.Equal(HttpStatusCode.BadRequest, Response.StatusCode);
        }

        [Fact]
        public async Task FailManagers()
        {
            var Response = await Client.GetAsync($"/v1/Managers");

            Assert.Equal(HttpStatusCode.BadRequest, Response.StatusCode);
        }

        [Fact]
        public async Task FailOrganizations()
        {
            var Response = await Client.GetAsync($"/v1/Organizations");

            Assert.Equal(HttpStatusCode.BadRequest, Response.StatusCode);
        }

        [Fact]
        public async Task FailPlayers()
        {
            var Response = await Client.GetAsync($"/v1/Players");

            Assert.Equal(HttpStatusCode.BadRequest, Response.StatusCode);
        }

        [Fact]
        public async Task FailTeams()
        {
            var Response = await Client.GetAsync($"/v1/Teams");

            Assert.Equal(HttpStatusCode.BadRequest, Response.StatusCode);
        }

        [Fact]
        public async Task FailTests()
        {
            var Response = await Client.GetAsync($"/v1/Tests");

            Assert.Equal(HttpStatusCode.BadRequest, Response.StatusCode);
        }

        [Fact]
        public async Task FailUsers()
        {
            var Response = await Client.GetAsync($"/v1/Users");

            Assert.Equal(HttpStatusCode.BadRequest, Response.StatusCode);
        }
    }
}
