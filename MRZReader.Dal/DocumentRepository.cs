using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MRZReader.Core;

namespace MRZReader.Dal
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly MrzReaderDbContext _context;

        public DocumentRepository(MrzReaderDbContext context)
        {
            _context = context;
        }
        public DocumentRequest Add(DocumentRequest request)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Purpose of using transaction that we might want to add more things when we storing a document.
                    _context.Document.Add(request.Document);
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // No need to roll-back- automatic roll-back if something goes wrong
                    request.IsSuccessed = false;
                }
            }
            return request;
        }
        public IEnumerable<Document> GetAll()
        {
           return _context.Document.Include("User").ToList();
        }
    }
}
