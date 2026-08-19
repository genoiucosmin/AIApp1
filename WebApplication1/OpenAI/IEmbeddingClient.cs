namespace WebApplication1.OpenAI
{
    public interface IEmbeddingClient
    {
        Task<float[]> CreateEmbeddingAsync(string text);
    }

}
