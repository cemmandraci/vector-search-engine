using VectorSearch.Domain.Enums;

namespace VectorSearch.Domain.Entities;

public class Document
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public DocumentStatus Status { get; private set; } = DocumentStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int ChunkCount { get; private set; }
    
    public void MarkAsProcessing() => Status = DocumentStatus.Processing;

    public void MarkAsCompleted(int chunkCount)
    {
        Status = DocumentStatus.Completed;
        ChunkCount = chunkCount;
    }

    public void MarkAsFailed() => Status = DocumentStatus.Failed;
}