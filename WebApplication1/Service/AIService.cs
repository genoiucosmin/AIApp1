using WebApplication1.OpenAI;

namespace WebApplication1.Service
{
    public class AiService
    {
        private readonly EmbeddingIndex _index;
        private readonly IOpenAiClient _ai;

        public AiService(EmbeddingIndex index, IOpenAiClient ai)
        {
            _index = index;
            _ai = ai;
        }

        public async Task<string> Ask(string question)
        {
            var relevant = await _index.SearchAsync(question);

            var context = string.Join(
                "\n\n",
                relevant.Select(c =>
                    $"Source: {c.Source}\n{c.Content}"));

            var systemPrompt =
                "You are an assistant that answers ONLY using the provided context.\n" +
                "If the answer is not present, say you don't know.\n\n" +
                "Context:\n" + context;

            return await _ai.AskAsync(systemPrompt, question);
        }
    }


}
