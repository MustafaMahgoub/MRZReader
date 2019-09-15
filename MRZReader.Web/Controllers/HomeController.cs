using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MRZReader.Core;
using MRZReader.Web.ViewModels;

namespace MRZReader.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly DocumentStorageSettings _documentStorageSettings;
        private readonly ILogger _logger;

        public HomeController(
            ILogger<HomeController> logger, 
            IMediator mediator, 
            IHostingEnvironment hostingEnvironment, 
            IOptions<DocumentStorageSettings> documentStorageSettings, 
            IHttpClientFactory httpClientFactory)
        {
            _mediator = mediator;
            _hostingEnvironment = hostingEnvironment;
            _documentStorageSettings = documentStorageSettings.Value;
            _HttpClientFactory = httpClientFactory;
            _logger = logger;
        }
        internal async Task<string> TestAuthentication()
        {
            string responseString = String.Empty;
            try
            {
                var client = _HttpClientFactory.CreateClient("TruliooClient");

                var request = new HttpRequestMessage(HttpMethod.Get, "connection/v1/testauthentication");
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {

                }
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
            }
            return responseString;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            try
            {
                TestAuthentication();
                return View();
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                return View("Error",e.Message.ToString());
            }
        }
        [HttpGet]
        public IActionResult Upload()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                return View("Error", e.Message.ToString());
            }
        }
        public async Task<IActionResult> Upload(DocumentViewModel model)
        {
            try
            {
                var isValidModel = ModelState.IsValid;
                Log($"File Upload- ModelState.IsValid: {isValidModel}");

                if (isValidModel)
                {
                    if (model.Document != null)
                    {
                        var _sourceFilePath = Path.Combine(_hostingEnvironment.WebRootPath, _documentStorageSettings.SourceFilePath);
                        var _outputFilePath = Path.Combine(_hostingEnvironment.WebRootPath, _documentStorageSettings.OutputFilePath);

                        Log($"File Upload- SourceFilePath: {_sourceFilePath}");
                        Log($"File Upload- OutputFilePath: {_outputFilePath}");

                        DocumentRequest request = new DocumentRequest()
                        {
                            SourceFolder = _sourceFilePath,
                            DestinationFolder = _outputFilePath,
                            OriginalFile = model.Document
                        };

                        await _mediator.Send(request);

                        if (request.IsSuccessed)
                            return RedirectToAction("Success");

                        Log($"Redirecting user to Error view - Request.IsSuccessed: {request.IsSuccessed}");
                        return RedirectToAction("Error");
                    }
                }
                return View();
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                Log($"Redirecting user to Error view");
                return View("Error", e.Message.ToString());
            }
        }
        public IActionResult Success()
        {
            return View();
        }
        private void Log(string msg, bool isEception = false)
        {
            if (isEception)
            {
                _logger.LogError($"[MRZ_Logs] {msg}.");
            }
            else
            {
                _logger.LogTrace($"[MRZ_Logs] {msg}.");
            }
        }
    }
}
