namespace WebApplication1.OpenAI
{
    public interface IOpenAiClient
    {
        Task<string> AskAsync(string systemPrompt, string question);
    }
}
