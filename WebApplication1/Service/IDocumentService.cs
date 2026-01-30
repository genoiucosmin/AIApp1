using WebApplication1.Model;

namespace WebApplication1.Service
{
    public interface IDocumentService
    {
        string LoadAll();
        IReadOnlyList<DocumentChunk> LoadChunks();
    }
}
