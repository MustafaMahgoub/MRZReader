namespace MRZReader.Core
{
    public interface IMRZReaderCache
    {
        void Update(int documentId, DocumentRequest request);
        DocumentRequest GetDocumentRequestInfo(int documentId);
        void Unlink(int documentId);
    }
}
