//using Microsoft.AspNetCore.Mvc;
//using MRZReader.Core;

//namespace MRZReader.Web.Controllers
//{
//    public class DocumentController : Controller
//    {
//        private IDocumentRepository _documentRepository;

//        public DocumentController(IDocumentRepository documentRepository)
//        {
//            _documentRepository = documentRepository;
//        }

//        public IActionResult Index()
//        {
//            var model = _documentRepository.GetAll();
//            return View(model);
//        }
//    }
//}