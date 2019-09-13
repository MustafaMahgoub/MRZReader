namespace MRZReader.Core
{
    public interface IDataExtractor
    {
        DocumentRequest Extract(DocumentRequest request);
    }
}
