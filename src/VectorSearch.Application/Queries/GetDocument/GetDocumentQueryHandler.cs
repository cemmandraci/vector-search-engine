using MediatR;
using VectorSearch.Application.DTOs;
using VectorSearch.Application.Exceptions;
using VectorSearch.Domain.Interfaces;

namespace VectorSearch.Application.Queries.GetDocument;

public class GetDocumentQueryHandler : IRequestHandler<GetDocumentQuery, IndexDocumentDto>
{
    private readonly IDocumentRepository _documentRepository;

    public GetDocumentQueryHandler(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<IndexDocumentDto> Handle(
        GetDocumentQuery request,
        CancellationToken cancellationToken)
    {
        var document = await _documentRepository
            .GetByIdAsync(request.Id, cancellationToken);

        if (document is null)
            throw new DocumentNotFoundException(request.Id);

        return new IndexDocumentDto
        {
            DocumentId = document.Id,
            Title = document.Title,
            ChunkCount = document.ChunkCount,
            Status = document.Status.ToString()
        };
    }
}