namespace VectorSearch.Application.DTOs;

public class IndexDocumentDto
{
    public Guid DocumentId { get; init; }
    public string Title { get; init; } = string.Empty;
    public int ChunkCount { get; init; }
    public string Status { get; init; } = string.Empty;
}