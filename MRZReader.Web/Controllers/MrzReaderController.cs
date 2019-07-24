using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRZReader.Core;

namespace MRZReader.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MrzReaderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MrzReaderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("")]
        [HttpPost]
        public async Task<OkObjectResult> Post(MrzDocumentRequest request)
        {
            Core.MrzDocumentRequest request1 = new Core.MrzDocumentRequest()
            {

            };
            return Ok(await _mediator.Send(request1));

            //return Ok();
            //return Ok(await _mediator.Send(request ?? new MrzDocumentRequest()));
        }

    }
}