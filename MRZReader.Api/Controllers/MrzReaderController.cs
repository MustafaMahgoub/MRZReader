using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MRZReader.Core;

namespace MRZReader.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MrzReaderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MrzReaderController()
        {

        }

        [Route("")]
        [HttpGet]
        public async Task<OkObjectResult> GetTrip([FromQuery]MrzDocumentRequest request)
        {
            return Ok(await _mediator.Send(request ?? new MrzDocumentRequest()));
        }
    }
}