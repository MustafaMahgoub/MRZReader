using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MRZReader.Core;
using MRZReader.Core;
using MRZReader.Dal;
using System.IO;
using MRZReader.Web.ViewModels;

namespace MRZReader.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IHttpClientFactory _HttpClientFactory;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly DocumentStorageSettings _documentStorageSettings;

        public HomeController(IMediator mediator, IHostingEnvironment hostingEnvironment, IOptions<DocumentStorageSettings> documentStorageSettings, IHttpClientFactory httpClientFactory)
        {
            _mediator = mediator;
            _hostingEnvironment = hostingEnvironment;
            _documentStorageSettings = documentStorageSettings.Value;
            _HttpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(DocumentViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //string fileName = null;
                    if (model.Document != null)
                    {
                        //var rootFolder = UploadDocument(model, out fileName);

                        var _uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, _documentStorageSettings.SourceFilePath);

                        // Make sure the file name is unique, otherwise if we upload the same file, it will override the existing one.
                        var fileName = Guid.NewGuid().ToString() + "_" + model.Document.FileName;
                        string sourceFilePath = Path.Combine(_uploadsFolder, fileName);

                        //Create the document.
                        using (FileStream fileStream = System.IO.File.Create(sourceFilePath))
                        {
                            model.Document.CopyTo(fileStream);
                        }
                        if (fileName == null)
                        {
                            // Delete the file, Just in case if the file was uploaded
                            DeleteDocument(fileName);
                            return RedirectToAction("Error", new { message = "Something went wrong, please try again" });
                        }
                        var outputFilePath = Path.Combine(_hostingEnvironment.WebRootPath, _documentStorageSettings.OutputFilePath);
                        var response = await PostToApi(sourceFilePath, outputFilePath);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Success", new { id = 10 });
                        }
                        else
                        {
                            return RedirectToAction("Error", new { message = "Error"});
                        }
                    }
                }
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", new { message = e.Message });
            }
        }
        private void DeleteDocument(string fileName)
        {
            try
            {
                if (System.IO.File.Exists(Path.Combine(_documentStorageSettings.OutputFilePath, fileName)))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(Path.Combine(_documentStorageSettings.OutputFilePath, fileName));
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private async Task<HttpResponseMessage> PostToApi(string sourceFilePath, string outputFilePath)
        {
            MrzDocumentRequest request = new MrzDocumentRequest()
            {
                SourceFilePath = sourceFilePath,
                OutputFilePath = outputFilePath
            };
            var client = _HttpClientFactory.CreateClient("GitHubClient");
            return await client.PostAsJsonAsync("api/MrzReader", request); 
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
