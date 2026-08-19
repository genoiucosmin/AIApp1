using System.Text.Json;

namespace WebApplication1.OpenAI
{
    public class OpenAiEmbeddingClient : IEmbeddingClient
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public OpenAiEmbeddingClient(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["OpenAi:ApiKey"];
        }

        public async Task<float[]> CreateEmbeddingAsync(string text)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            var body = new
            {
                model = "text-embedding-3-small",
                input = text
            };

            var response = await _http.PostAsJsonAsync(
                "https://api.openai.com/v1/embeddings",
                body);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();

            var vector = json
                .GetProperty("data")[0]
                .GetProperty("embedding")
                .EnumerateArray()
                .Select(x => x.GetSingle())
                .ToArray();

            return vector;
        }
    }

}
