using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MRZReader.Core;
using MRZReader.Web.ViewModels;

namespace MRZReader.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly DocumentStorageSettings _documentStorageSettings;
        private readonly ILogger _logger;

        public HomeController(
            ILoggerFactory loggerFactory, 
            IMediator mediator, 
            IHostingEnvironment hostingEnvironment, 
            IOptions<DocumentStorageSettings> documentStorageSettings, 
            IHttpClientFactory httpClientFactory)
        {
            _mediator = mediator;
            _hostingEnvironment = hostingEnvironment;
            _documentStorageSettings = documentStorageSettings.Value;
            _HttpClientFactory = httpClientFactory;
            _logger = loggerFactory.CreateLogger(nameof(HomeController));
        }
        public IActionResult Index()
        {
            _logger.LogInformation($"Index Called");
            return View();
        }
        [HttpGet]
        public IActionResult Upload()
        {
            _logger.LogInformation($"Upload Called");
            return View();
        }
        public async Task<IActionResult> Upload(DocumentViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation($"ModelState.IsValid {ModelState.IsValid}");

                    //string fileName = null;
                    if (model.Document != null)
                    {
                        var _sourceFilePath = Path.Combine(_hostingEnvironment.WebRootPath, _documentStorageSettings.SourceFilePath);
                        var _outputFilePath = Path.Combine(_hostingEnvironment.WebRootPath, _documentStorageSettings.OutputFilePath);
                        await _mediator.Send(new MrzDocumentRequest()
                        {
                            SourceFolder = _sourceFilePath,
                            DestinationFolder = _outputFilePath,
                            Document = model.Document,
                            ShouldContinue = true
                        });
                        return RedirectToAction("Success");
                    }
                }
                return View();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Exception {e.Message}");
                return RedirectToAction("Error", new { message = e.Message });
            }
        }
        public IActionResult Error()
        {
            return View();
        }
        public IActionResult Success()
        {
            return View();
        }
    }
}
