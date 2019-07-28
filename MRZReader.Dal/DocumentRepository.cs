using System;
using System.Collections.Generic;
using System.Text;
using MRZReader.Core;
using MRZReader.Core.Interfaces;

namespace MRZReader.Dal
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly MrzReaderDbContext _context;

        public DocumentRepository(MrzReaderDbContext context)
        {
            this._context = context;
        }
        public Document Add(Document testDocument)
        {
            try
            {
                var doc = this._context.Document;
                testDocument = new Document()
                {
                    DocumentLocation = "TestLocation",
                    DocumentExtension = "txt"
                };
                this._context.Document.Add(testDocument);
                _context.SaveChanges();
                return testDocument;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
