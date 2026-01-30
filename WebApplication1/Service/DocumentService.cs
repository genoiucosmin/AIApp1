using WebApplication1.Model;

namespace WebApplication1.Service
{
    public class DocumentService : IDocumentService
    {
        private const int ChunkSize = 300;
        public string LoadAll()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Docs");

            if (!Directory.Exists(path))
                return string.Empty;

            var files = Directory.GetFiles(path);
            return string.Join("\n\n", files.Select(File.ReadAllText));
        }

        public IReadOnlyList<DocumentChunk> LoadChunks()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Docs");
            var chunks = new List<DocumentChunk>();
            if (!Directory.Exists(path))
                return chunks;

            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                var text = File.ReadAllText(file);
                
                for(int i = 0; i< text.Length; i+=ChunkSize)
                {
                    var length = Math.Min(ChunkSize, text.Length - i);
                    var chunkText = text.Substring(i, length);

                    chunks.Add(new DocumentChunk
                    (
                         Path.GetFileName(file),
                         chunkText));
                }
            }
            return chunks;
        }
    }
}
