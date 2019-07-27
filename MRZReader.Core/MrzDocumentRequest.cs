using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRZReader.Core
{
    public class MrzDocumentRequest : IRequest
    {
        public string SourceFilePath { get; set; }
    }


    public class TestDocument
    {
        public int Id{ get; set; }
        public string Extension { get; set; }
        public string SourceFilePath { get; set; }
    }
}
