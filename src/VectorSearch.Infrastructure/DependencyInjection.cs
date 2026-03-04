using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Qdrant.Client;
using VectorSearch.Application.Services;
using VectorSearch.Domain.Interfaces;
using VectorSearch.Infrastructure.Repositories;
using VectorSearch.Infrastructure.Services;

namespace VectorSearch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient("Gemini", client =>
        {
            client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
            client.Timeout = TimeSpan.FromSeconds(30);
        });
        
        var qdrantHost = configuration["Qdrant:Host"] ?? "localhost";
        var qdrantPort = int.Parse(configuration["Qdrant:Port"] ?? "6334");
        
        services.AddSingleton(new QdrantClient(qdrantHost, qdrantPort));
        
        services.AddSingleton<IDocumentRepository, InMemoryDocumentRepository>();
        services.AddScoped<IVectorRepository, QdrantVectorRepository>();
        
        services.AddScoped<IEmbeddingService, GeminiEmbeddingService>();
        
        services.AddSingleton(new ChunkingService(chunkSize: 500, overlapSize: 50));

        return services;
    }
}