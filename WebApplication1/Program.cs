using WebApplication1.OpenAI;
using WebApplication1.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application services before building the app
builder.Services.AddSingleton<IDocumentService, DocumentService>();
builder.Services.AddHttpClient<IOpenAiClient, OpenAiClient>();
builder.Services.AddHttpClient<IEmbeddingClient, OpenAiEmbeddingClient>();
builder.Services.AddSingleton<EmbeddingIndex>();

builder.Services.AddScoped<AiService>();

builder.Services.AddSingleton<ChunkSelector>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
