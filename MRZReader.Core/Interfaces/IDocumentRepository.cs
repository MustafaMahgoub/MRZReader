using System;
using System.Collections.Generic;
using System.Text;

namespace MRZReader.Core.Interfaces
{
    public interface IDocumentRepository
    {
        Document Add(Document Document);
    }
}
