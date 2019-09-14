using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MRZReader.Core;
using ReflectionIT.Mvc.Paging;

namespace MRZReader.Web.Controllers
{
    public class DocumentController : Controller
    {
        private IDocumentRepository _documentRepository;

        public DocumentController(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }
        public IActionResult Index()
        {
            var model = _documentRepository.GetAll().ToList().OrderBy(p => p.FileFullName);
            return View(model);
        }
    }
}