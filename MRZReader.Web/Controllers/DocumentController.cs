using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<FileStreamResult> Download(string filename)
        {
            // Future Dev improvements, store the extension and mime type along with document in the database.
            //var fileId=
            //var fileName=
            //var source = document.SourceFilePath;
            //var target = document.TargetFilePath;
            var path = filename;

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".xml", "application/xml"},
                {".pdf", "application/pdf"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"}
            };
        }
    }
}