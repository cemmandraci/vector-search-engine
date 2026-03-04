using MediatR;
using VectorSearch.Application.DTOs;

namespace VectorSearch.Application.Queries.GetDocument;

public record GetDocumentQuery(Guid Id) : IRequest<IndexDocumentDto>;