using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MRZReader.Core
{
    public interface ITruliooHandler
    {
        Task<DocumentRequest> Handle(DocumentRequest request);
    }
}
