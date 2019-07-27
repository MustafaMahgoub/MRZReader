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
using MRZReader.Core;
using MRZReader.Core.Interfaces;
using MRZReader.Dal;
using MRZReader.Web.Models;
using MRZReader.Web.ViewModels;

namespace MRZReader.Web.Controllers
{
    public class HomeController : Controller
    {
        private IDocumentRepository _repository;

        private readonly IMediator _mediator;
        static HttpClient client = new HttpClient();
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IMediator mediator, IHostingEnvironment hostingEnvironment, IDocumentRepository repository)
        {
            _mediator = mediator;
            _hostingEnvironment = hostingEnvironment;
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Upload()
        {
            try
            {
                _repository.Add(new TestDocument());
                return RedirectToAction("Success", new { id = 10 });
            }
            catch (Exception e)
            {
                return View();
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> Upload(DocumentViewModel model)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                if (model.Document != null)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Documents\\Src");

                    // Make sure the file name is unique, otherwise if we upload the same file, it will override the existing one.
                    fileName = Guid.NewGuid().ToString() + "_" + model.Document.FileName;

                    string filePath=Path.Combine(uploadsFolder, fileName);
                    model.Document.CopyTo(new FileStream(filePath,FileMode.Create));
                    await PostToApi(filePath);
                    return RedirectToAction("Success", new {id = 10});
                }
            }
            return View();
        }
        private static async Task PostToApi(string filePath)
        {
            client.BaseAddress = new Uri("https://localhost:44381/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Core.MrzDocumentRequest request = new Core.MrzDocumentRequest()
            {
                SourceFilePath = filePath
            };
            HttpResponseMessage response = await client.PostAsJsonAsync("api/MrzReader", request);
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
