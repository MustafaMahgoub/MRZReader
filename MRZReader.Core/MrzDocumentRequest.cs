using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRZReader.Core
{
    public class MrzDocumentRequest : IRequest
    {
        public string SourceFilePath { get; set; }
        public string OutputFilePath { get; set; }
    }
    public class Document
    {
        public int DocumentId { get; set; }
        public string DocumentExtension { get; set; }
        public string DocumentLocation { get; set; }
    }
}
