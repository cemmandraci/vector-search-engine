using MediatR;
using VectorSearch.Application.DTOs;

namespace VectorSearch.Application.Commands.IndexDocument;

public record IndexDocumentCommand(string Title, string Content) : IRequest<IndexDocumentDto>;