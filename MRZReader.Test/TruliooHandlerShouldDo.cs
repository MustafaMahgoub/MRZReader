using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using Microsoft.AspNetCore.Http;
using MRZReader.Core;
using MRZReader.Handlers;
using MRZReader.Handlers.KYCHandlers;
using System.Threading.Tasks;

namespace MRZReader.Test
{
    [TestClass]
    public class TruliooHandlerShouldDo
    {
        [TestMethod]
        public async Task TruliooHandlerTest()
        {
            var logger = Substitute.For<ILogger<TruliooHandler>>();
            var options = Substitute.For<IOptions<TruliooSettings>>();

            options.Value.Returns(new TruliooSettings()
            {
                Username = "MGMahgoub2012@Gmail.com",
                Password = "Mustafa99"
            });
            TruliooHandler handler = new TruliooHandler(logger, options);
            DocumentRequest request = new DocumentRequest()
            {
                SourceFolder = @"somepath",
                DestinationFolder = "somepath",
                OriginalFile = Substitute.For<IFormFile>()
            };
            var res=await handler.Handle(request);
        }
    }
}
