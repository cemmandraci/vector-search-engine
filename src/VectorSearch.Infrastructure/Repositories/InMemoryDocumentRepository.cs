using System.Collections.Concurrent;
using VectorSearch.Domain.Entities;
using VectorSearch.Domain.Interfaces;

namespace VectorSearch.Infrastructure.Repositories;

public class InMemoryDocumentRepository : IDocumentRepository
{
    private readonly ConcurrentDictionary<Guid, Document> _documents = new();
    
    public Task<Document> AddAsync(Document document, CancellationToken cancellationToken = default)
    {
        _documents[document.Id] = document;
        return Task.FromResult(document);
    }

    public Task<Document?> GetByIdAsync(Guid documentId, CancellationToken cancellationToken = default)
    {
        _documents.TryGetValue(documentId, out var document);
        return Task.FromResult(document);
    }

    public Task UpdateAsync(Document document, CancellationToken cancellationToken = default)
    {
        _documents[document.Id] = document;
        return Task.FromResult(true);
    }

    public Task<IReadOnlyList<Document>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Document> result = _documents.Values.ToList();
        return Task.FromResult(result);
    }
}