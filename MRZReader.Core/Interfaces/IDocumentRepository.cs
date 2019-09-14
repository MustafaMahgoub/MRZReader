using System.Collections.Generic;

namespace MRZReader.Core
{
    public interface IDocumentRepository
    {
        DocumentRequest Add(DocumentRequest request);
        IEnumerable<Document> GetAll();
    }
}
