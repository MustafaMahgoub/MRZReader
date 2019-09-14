using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MRZReader.Core;

namespace MRZReader.Dal
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly MrzReaderDbContext _context;
        private readonly ILogger _logger;

        public DocumentRepository(ILogger<DocumentRepository> logger, MrzReaderDbContext context)
        {
            _context = context;
            _logger = logger;
        }
        public DocumentRequest Add(DocumentRequest request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Purpose of using transaction that we might want to add more things when we storing a document.
                    request.Document.CreatedDate = DateTime.Now;
                    _context.Document.Add(request.Document);
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    // No need to roll-back- automatic roll-back if something goes wrong
                    request.IsSuccessed = false;
                    Log($"KO :Exception: {e.Message}", true);
                    throw;
                }
            }
            return request;
        }
        public IEnumerable<Document> GetAll()
        {
            try
            {
                return _context.Document.Include("User").ToList();
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                throw;
            }
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
