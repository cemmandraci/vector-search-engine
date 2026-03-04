using MediatR;
using VectorSearch.Application.DTOs;

namespace VectorSearch.Application.Queries.SearchDocuments;

public record SearchDocumentsQuery(string QueryText, int Limit = 5) : IRequest<IReadOnlyList<SearchResultDto>>;