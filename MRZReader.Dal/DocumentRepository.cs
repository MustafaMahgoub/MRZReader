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
        public TestDocument Add(TestDocument testDocument)
        {
            testDocument = new TestDocument()
            {
                SourceFilePath = "TestLocation",
                Extension = "txt"
            };
            _context.Documents.Add(testDocument);
            _context.SaveChanges();
            return testDocument;
        }
    }
}
