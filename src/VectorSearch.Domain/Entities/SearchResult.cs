namespace VectorSearch.Domain.Entities;

public class SearchResult
{
    public Guid ChunkId { get; init; }
    public Guid DocumentId { get; init; }
    public string DocumentTitle { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public float Score { get; init; }
    public int ChunkIndex { get; init; }
}