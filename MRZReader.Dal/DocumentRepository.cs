using System;
using System.Collections.Generic;
using MRZReader.Core;

namespace MRZReader.Dal
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly MrzReaderDbContext _context;

        public DocumentRepository(MrzReaderDbContext context)
        {
            this._context = context;
        }
        public Document Add(Document document)
        {
            try
            {
                _context.Document.Add(document);
                _context.SaveChanges();
                return document;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IEnumerable<Document> GetAll()
        {
            return _context.Document;
        }
    }
}
