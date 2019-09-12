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
                        var success=await _mediator.Send(new MrzDocumentRequest()
                        {
                            SourceFolder = _sourceFilePath,
                            DestinationFolder = _outputFilePath,
                            Document = model.Document,
                            ShouldContinue = true
                        });


                        //// Make sure the file name is unique, otherwise if we upload the same file, it will override the existing one.
                        //var fileName = Guid.NewGuid().ToString() + "_" + model.Document.FileName;
                        //string sourceFilePath = Path.Combine(_uploadsFolder, fileName);

                        ////Create the document.
                        //using (FileStream fileStream = System.IO.File.Create(sourceFilePath))
                        //{
                        //    model.Document.CopyTo(fileStream);
                        //}

                        //if (fileName == null)
                        //{
                        //    _logger.LogInformation($"fileName {fileName}");

                        //    // Delete the file, Just in case if the file was uploaded
                        //    DeleteDocument(fileName);
                        //    return RedirectToAction("Error", new { message = "Something went wrong, please try again" });
                        //}
                        //var outputFilePath = Path.Combine(_hostingEnvironment.WebRootPath, _documentStorageSettings.OutputFilePath);



                        //var response = await PostToApi(sourceFilePath, outputFilePath);
                        //if (response.IsSuccessStatusCode)
                        //{
                        //    _logger.LogInformation($"response.IsSuccessStatusCode {response.IsSuccessStatusCode}");
                        //    return RedirectToAction("Success", new { id = 10 });
                        //}
                        //else
                        //{
                        //    _logger.LogInformation($"response.IsSuccessStatusCode {response.IsSuccessStatusCode}");
                        //    return RedirectToAction("Error", new { message = "Error" });
                        //}
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











        //[HttpPost]
        //public async Task<IActionResult> Upload(DocumentViewModel model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _logger.LogInformation($"ModelState.IsValid {ModelState.IsValid}");

        //            //string fileName = null;
        //            if (model.Document != null)
        //            {

        //                var _uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, _documentStorageSettings.SourceFilePath);

        //                // Make sure the file name is unique, otherwise if we upload the same file, it will override the existing one.
        //                var fileName = Guid.NewGuid().ToString() + "_" + model.Document.FileName;
        //                string sourceFilePath = Path.Combine(_uploadsFolder, fileName);

        //                //Create the document.
        //                using (FileStream fileStream = System.IO.File.Create(sourceFilePath))
        //                {
        //                    model.Document.CopyTo(fileStream);
        //                }

        //                if (fileName == null)
        //                {
        //                    _logger.LogInformation($"fileName {fileName}");

        //                    // Delete the file, Just in case if the file was uploaded
        //                    DeleteDocument(fileName);
        //                    return RedirectToAction("Error", new { message = "Something went wrong, please try again" });
        //                }
        //                var outputFilePath = Path.Combine(_hostingEnvironment.WebRootPath, _documentStorageSettings.OutputFilePath);
        //                await _mediator.Send(new MrzDocumentRequest()
        //                {
        //                    SourceFilePath = sourceFilePath,
        //                    OutputFilePath = outputFilePath
        //                });


        //                //var response = await PostToApi(sourceFilePath, outputFilePath);
        //                //if (response.IsSuccessStatusCode)
        //                //{
        //                //    _logger.LogInformation($"response.IsSuccessStatusCode {response.IsSuccessStatusCode}");
        //                //    return RedirectToAction("Success", new { id = 10 });
        //                //}
        //                //else
        //                //{
        //                //    _logger.LogInformation($"response.IsSuccessStatusCode {response.IsSuccessStatusCode}");
        //                //    return RedirectToAction("Error", new { message = "Error"});
        //                //}
        //            }
        //        }
        //        return View();
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogInformation($"Exception {e.Message}");
        //        return RedirectToAction("Error", new { message = e.Message});
        //    }
        //}
        //private void DeleteDocument(string fileName)
        //{
        //    try
        //    {
        //        if (System.IO.File.Exists(Path.Combine(_documentStorageSettings.OutputFilePath, fileName)))
        //        {
        //            // If file found, delete it    
        //            System.IO.File.Delete(Path.Combine(_documentStorageSettings.OutputFilePath, fileName));
        //        }
        //    }
        //    catch (IOException e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}
        //private async Task<HttpResponseMessage> PostToApi(string sourceFilePath, string outputFilePath)
        //{
        //    MrzDocumentRequest request = new MrzDocumentRequest()
        //    {
        //        SourceFilePath = sourceFilePath,
        //        OutputFilePath = outputFilePath
        //    };
        //    var client = _HttpClientFactory.CreateClient("MRZClient");
        //    return await client.PostAsJsonAsync("api/MrzReader", request); 
        //}
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
