using Microsoft.AspNetCore.Http;

namespace MRZReader.Web.ViewModels
{
    public class DocumentViewModel
    {
        public IFormFile Document{ get; set; }
    }
}
