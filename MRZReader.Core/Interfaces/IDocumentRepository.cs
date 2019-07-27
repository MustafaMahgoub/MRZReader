using System;
using System.Collections.Generic;
using System.Text;

namespace MRZReader.Core.Interfaces
{
    public interface IDocumentRepository
    {
        TestDocument Add(TestDocument testDocument);
    }
}
