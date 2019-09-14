using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MRZReader.Core;
using ReflectionIT.Mvc.Paging;

namespace MRZReader.Web.Controllers
{
    public class DocumentController : Controller
    {
        private IDocumentRepository _documentRepository;
        private readonly ILogger _logger;

        public DocumentController(
            ILogger<DocumentController> logger, 
            IDocumentRepository documentRepository, 
            ILoggerFactory loggerFactory)
        {
            _documentRepository = documentRepository;
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
                var model = _documentRepository.GetAll().ToList().OrderBy(p => p.FileFullName);
                return View(model);
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                return View("Error", e.Message.ToString());
            }
        }
        public ActionResult Download(string filename)
        {
            try
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
                     stream.CopyTo(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                return View("Error", e.Message.ToString());
            }
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
        private void Log(string msg, bool isEception = false)
        {
            if (isEception)
            {
                _logger.LogError($"[MRZ_Logs] {msg}.");
            }
            else
            {
                _logger.LogTrace($"[MRZ_Logs] {msg}.");
            }
        }
    }
}