using WebApplication1.OpenAI;

namespace WebApplication1.Service
{
    public class AiService
    {
        private readonly IDocumentService _docs;
        private readonly IOpenAiClient _ai;
        private readonly ChunkSelector _selector;

        public AiService(IDocumentService docs, IOpenAiClient ai, ChunkSelector selector)
        {
            _docs = docs;
            _ai = ai;
            _selector = selector;
        }

        public Task<string> Ask(string question)
        {
            //var context = _docs.LoadAll();
            var chunks = _docs.LoadChunks();
            var relevant = _selector.SelectRelevant(chunks, question);

            var context = string.Join("\n\n", relevant.Select(c => $"Source : {c.Source}\n{c.Content}"));

            var systemPrompt =
                "You are an assistant that answers ONLY using the provided documents.\n" +
                "If the answer is not present, say you don't know.\n\n" +
                "Context:\n" + context;

            return _ai.AskAsync(systemPrompt, question);
        }
    }

}
