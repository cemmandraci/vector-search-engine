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
    // "embeddings" olmalı, "Embeddings" property adı JSON ile eşleşmeli
    public List<EmbeddingValues> Embeddings { get; init; } = [];
}