using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace AnilistClone.IntegrationTest
{
    public class Media_RequestPipeline_IntegrationTests
    {
        [Fact]
        public async Task Returns_ServiceUnavailable_When_ExternalApiFails()
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
            Console.WriteLine($"Status: {response.StatusCode}");
            Console.WriteLine($"Body: {await response.Content.ReadAsStringAsync()}");
            Console.WriteLine($"WireMock requests: {mockServer.LogEntries.Count}");

            Assert.Equal(System.Net.HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }
    }
}
