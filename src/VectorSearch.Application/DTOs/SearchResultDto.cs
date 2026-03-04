namespace VectorSearch.Application.DTOs;

public class SearchResultDto
{
    public Guid DocumentId { get; init; }
    public string DocumentTitle { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public float Score { get; init; }
    public int ChunkIndex { get; init; }
}