using System.Collections.Generic;
using System.Xml;

namespace MRZReader.Core
{
    public interface IDocumentRepository
    {
        DocumentRequest Add(DocumentRequest request);
        IEnumerable<Document> GetAll();
    }
}
