using System;
using System.Collections.Generic;
using System.Text;
using MRZReader.Core;
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
                //var doc = this._context.Document;
                //document = new Document()
                //{
                //    DocumentLocation = "TestLocation",
                //    DocumentExtension = "txt"
                //};
                this._context.Document.Add(document);
                _context.SaveChanges();
                return document;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
