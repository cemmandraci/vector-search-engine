using MediatR;
using VectorSearch.Application.DTOs;
using VectorSearch.Domain.Interfaces;

namespace VectorSearch.Application.Queries.SearchDocuments;

public class SearchDocumentsQueryHandler : IRequestHandler<SearchDocumentsQuery, IReadOnlyList<SearchResultDto>>
{
    private readonly IVectorRepository _vectorRepository;
    private readonly IEmbeddingService _embeddingService;

    public SearchDocumentsQueryHandler(
        IVectorRepository vectorRepository,
        IEmbeddingService embeddingService)
    {
        _vectorRepository = vectorRepository;
        _embeddingService = embeddingService;
    }

    public async Task<IReadOnlyList<SearchResultDto>> Handle(SearchDocumentsQuery request, CancellationToken cancellationToken)
    {
        var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(request.QueryText, cancellationToken);

        var results = await _vectorRepository.SearchAsync(queryEmbedding, request.Limit, cancellationToken);

        return results.Select(r => new SearchResultDto
        {
            DocumentId = r.DocumentId,
            DocumentTitle = r.DocumentTitle,
            Content = r.Content,
            Score = r.Score,
            ChunkIndex = r.ChunkIndex
        }).ToList();
    }
}