using WebApplication1.Helper;
using WebApplication1.Model;
using WebApplication1.OpenAI;

namespace WebApplication1.Service
{
    public class EmbeddingIndex
    {
        private readonly IDocumentService _docs;
        private readonly IEmbeddingClient _embedder;

        private List<EmbeddedChunk>? _index;

        public EmbeddingIndex(IDocumentService docs, IEmbeddingClient embedder)
        {
            _docs = docs;
            _embedder = embedder;
        }

        public async Task BuildAsync()
        {
            if (_index != null)
                return;

            var chunks = _docs.LoadChunks();
            _index = new List<EmbeddedChunk>();

            foreach (var chunk in chunks)
            {
                var vector = await _embedder.CreateEmbeddingAsync(chunk.Content);

                _index.Add(new EmbeddedChunk(
                    chunk.Source,
                    chunk.Content,
                    vector));
            }
        }

        public async Task<IReadOnlyList<EmbeddedChunk>> SearchAsync(string question, int top = 3)
        {
            if (_index == null)
                await BuildAsync();

            var queryVector = await _embedder.CreateEmbeddingAsync(question);

            return _index!
                .Select(c => new
                {
                    Chunk = c,
                    Score = VectorMath.CosineSimilarity(queryVector, c.Vector)
                })
                .OrderByDescending(x => x.Score)
                .Take(top)
                .Select(x => x.Chunk)
                .ToList();
        }
    }

}
