using MediatR;
using Microsoft.AspNetCore.Http;

namespace MRZReader.Core
{
    /// <summary>
    /// Is a generic request class that can be used for document proccessing such as MRZ documents
    /// </summary>
    /// <seealso cref="MediatR.IRequest" />
    public class DocumentRequest : IRequest
    {
        public string SourceFolder { get; set; }
        public string DestinationFolder { get; set; }
        public string FileUniqueName { get; set; }
        //public IFormFile OriginalFile { get; set; }
        public bool ShouldContinue { get; set; }
        public bool IsSuccessed { get; set; }
        public Document Document { get; set; } = new Document();


        //public string SourceFilePath { get; set; }
        //public string DocumentName { get; set; }
        //public string OutputFilePath { get; set; }
    }
}
