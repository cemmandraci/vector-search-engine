namespace VectorSearch.Domain.Entities;

public class DocumentChunk
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid DocumentId { get; init; }
    public string Content { get; init; } = string.Empty;
    public int ChunkIndex { get; init; }
    public int StartPosition { get; init; }
    public float[]? Embedding { get; private set; }

    public void SetEmbedding(float[] embedding)
    {
        if (embedding.Length == 0)
            throw new ArgumentException("Embedding cannot be empty");
        Embedding = embedding;
    }
}