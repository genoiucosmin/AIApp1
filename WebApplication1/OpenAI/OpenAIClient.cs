using System.Net.Http.Headers;
using System.Text.Json;
using System.Net;

namespace WebApplication1.OpenAI
{
    public class OpenAiClient : IOpenAiClient
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public OpenAiClient(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["OpenAi:ApiKey"];
        }

        public async Task<string> AskAsync(string systemPrompt, string question)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            var body = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = question }
                }
            };

            const int maxRetries = 3;
            int attempt = 0;

            while (true)
            {
                attempt++;
                using var response = await _http.PostAsJsonAsync(
                    "https://api.openai.com/v1/chat/completions", body);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadFromJsonAsync<JsonElement>();

                    return json
                        .GetProperty("choices")[0]
                        .GetProperty("message")
                        .GetProperty("content")
                        .GetString();
                }

                // If rate limited, respect Retry-After and retry with backoff
                if (response.StatusCode == (HttpStatusCode)429 && attempt <= maxRetries)
                {
                    TimeSpan delay = TimeSpan.FromSeconds(Math.Pow(2, attempt)); // exponential backoff

                    if (response.Headers.RetryAfter != null)
                    {
                        if (response.Headers.RetryAfter.Delta.HasValue)
                        {
                            delay = response.Headers.RetryAfter.Delta.Value;
                        }
                        else if (response.Headers.RetryAfter.Date.HasValue)
                        {
                            var retryDate = response.Headers.RetryAfter.Date.Value;
                            var now = DateTimeOffset.UtcNow;
                            var computed = retryDate - now;
                            if (computed > TimeSpan.Zero)
                                delay = computed;
                        }
                    }

                    await Task.Delay(delay);
                    continue;
                }

                // Non-success and not retrying (or retries exhausted): include response body for diagnostics
                var content = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"OpenAI request failed with status {(int)response.StatusCode} ({response.ReasonPhrase}). Response: {content}");
            }
        }
    }
}
