using AnilistClone;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;


namespace Anilistclone.Test
{
    public class GlobalExceptionMiddlewareTest
    {



        [Fact]

        public async Task Test1()
        {

            using var mockServer = WireMockServer.Start();

            mockServer.Given(Request.Create().WithPath("/graphql").UsingPost()).RespondWith(Response.Create().WithStatusCode(503).WithBody(@"{ msg: ""Service Unavailable""}"));




            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration( configBuilder =>
                {
                    configBuilder.AddInMemoryCollection(new Dictionary<string, string?> { ["AnilistApiUrl"] = mockServer.Url + "/graphql" });
                });
            });

            var client = factory.CreateClient();

            var response = await client.GetAsync("/Show/get-show?id=1");


            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);

        }
    }
}
