using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace AnilistClone.IntegrationTest
{
    public class GlobalExceptionMiddlewareTest
    {
        [Fact]
        public async Task Test1()
        {
            using var mockServer = WireMockServer.Start();

            mockServer
                .Given(Request.Create().WithPath("/graphql").UsingPost())
                .RespondWith(
                    Response
                        .Create()
                        .WithStatusCode(503)
                        .WithBody(@"{ msg: ""Service Unavailable""}")
                );

            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration(configBuilder =>
                {
                    configBuilder.AddInMemoryCollection(
                        new Dictionary<string, string?>
                        {
                            ["AnilistApiUrl"] = mockServer.Url + "/graphql",
                        }
                    );
                });
            });

            var client = factory.CreateClient();

            var response = await client.GetAsync("/Media/1");

            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
