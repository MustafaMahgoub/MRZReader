using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MRZReader.Core;
using MRZReader.Handlers;
using MRZReader.Handlers.KYCHandlers;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MRZReader.Test
{
    [TestClass]
    public class TruliooHandlerShouldDo
    {
        private TruliooHandler handler;

        [TestInitialize]
        public void TestInitialize()
        {
            var logger = Substitute.For<ILogger<TruliooHandler>>();
            var options = Substitute.For<IOptions<TruliooSettings>>();
            var htttpClientFactory = Substitute.For<IHttpClientFactory>();
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://gateway.trulioo.com/trial/"),
                Timeout = TimeSpan.FromSeconds(30)
            };
            client.DefaultRequestHeaders.Add("x-trulioo-api-key", "42f5e1dabd01c87d68dad5790b0ef3a6");
            htttpClientFactory.CreateClient("TruliooClient").Returns(client);

            options.Value.Returns(new TruliooSettings()
            {
                Username = "MGMahgoub2012@Gmail.com",
                Password = "Mustafa99"
            });
            handler = new TruliooHandler(logger, options, htttpClientFactory);
        }
        [TestMethod]
        public async Task TruliooHandlerTest()
        {
            var res = await handler.SayHelloAsync();
            Assert.AreEqual("\"Hello Joe Napoli\"", res);
        }
        [TestMethod]
        public async Task TestAuthenticationTest()
        {
            DocumentRequest request = new DocumentRequest();
            var response = await handler.TestAuthentication(request);
            Assert.IsTrue(request.IsTruliooAuthenticated);
        }
    }
}
