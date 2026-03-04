using VectorSearch.Domain.Entities;

namespace VectorSearch.Domain.Interfaces;

public interface IVectorRepository
{
    Task UpsertChunkAsync(IReadOnlyList<DocumentChunk> chunks, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SearchResult>> SearchAsync(float[] queryEmbedding, int limit = 5, CancellationToken cancellationToken = default);
    Task DeleteByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default);
    Task EnsureCollectionExistsAsync(CancellationToken cancellationToken = default);
}