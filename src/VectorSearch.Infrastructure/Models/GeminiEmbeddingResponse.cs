namespace VectorSearch.Infrastructure.Models;

public class GeminiEmbeddingResponse
{
    public EmbeddingValues Embedding { get; init; } = new();
}

public class EmbeddingValues
{
    public float[] Values { get; init; } = [];
}

public class GeminiBatchEmbeddingResponse
{
    public List<GeminiEmbeddingResponse> Embeddings { get; init; } = [];
}