using WebApplication1.Model;

namespace WebApplication1.Service
{
    public class ChunkSelector
    {
        //splits the question in words
        // searches how many of those words does each chunk have
        //orders the chunks by the number of matching words
        //only returns the top maxChunks chunks. this is so not too much text is sent to the LLM
        public IReadOnlyList<DocumentChunk> SelectRelevant(IReadOnlyList<DocumentChunk> chunks, string question, int maxChunks = 3)
        {
            var keywords = question
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(k => k.ToLowerInvariant())
                .ToHashSet();

            return chunks.Select(c => new
            {
                Chunk = c,
                Score = keywords.Count(k => c.Content.ToLowerInvariant().Contains(k))
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Take(maxChunks)
                .Select(x => x.Chunk)
                .ToList();
        }
    }
}
