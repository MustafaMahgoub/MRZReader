using System.Threading.Tasks;
using MediatR;
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
        public async Task<OkObjectResult> Post(DocumentRequest request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}