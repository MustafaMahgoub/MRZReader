using System.Collections.Generic;

namespace MRZReader.Core
{
    public interface IDocumentRepository
    {
        Document Add(Document Document);
        IEnumerable<Document> GetAll();
    }
}
