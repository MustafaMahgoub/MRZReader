using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MRZReader.Web.Models;

namespace MRZReader.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        static HttpClient client = new HttpClient();

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Privacy()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://localhost:44381/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Core.MrzDocumentRequest request = new Core.MrzDocumentRequest()
            {

            };
            HttpResponseMessage response = await client.PostAsJsonAsync("api/MrzReader", request);
            
            //await _mediator.Send(request);
            //return Ok(await _mediator.Send(request ?? new CommercialTripRequest()));
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
