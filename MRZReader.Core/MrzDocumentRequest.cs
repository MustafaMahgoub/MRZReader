using System.Configuration;
using MediatR;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http;

namespace MRZReader.Core
{
    public class MrzDocumentRequest : IRequest
    {
        public string SourceFolder { get; set; }
        public string DestinationFolder { get; set; }
        public string DocumentName { get; set; }
        public string FileUniqueName { get; set; }

        //public string SourceFilePath { get; set; }
        public string OutputFilePath { get; set; }

        
        public string SourceFilePath { get; set; }
        public IFormFile Document { get; set; }
        public bool ShouldContinue { get; set; }
    }
    public class Document
    {
        public int DocumentId { get; set; }
        public string DocumentExtension { get; set; }
        public string DocumentLocation { get; set; }
    }
}
