using WebApplication1.OpenAI;

namespace WebApplication1.Service
{
    public class AiService
    {
        private readonly IDocumentService _docs;
        private readonly IOpenAiClient _ai;

        public AiService(IDocumentService docs, IOpenAiClient ai)
        {
            _docs = docs;
            _ai = ai;
        }

        public Task<string> Ask(string question)
        {
            var context = _docs.LoadAll();

            var systemPrompt =
                "You are an assistant that answers ONLY using the provided documents.\n" +
                "If the answer is not present, say you don't know.\n\n" +
                "Documents:\n" + context;

            return _ai.AskAsync(systemPrompt, question);
        }
    }

}
