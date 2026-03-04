using MediatR;
using Microsoft.AspNetCore.Mvc;
using VectorSearch.API.Models;
using VectorSearch.Application.Commands.IndexDocument;
using VectorSearch.Application.DTOs;
using VectorSearch.Application.Queries.GetDocument;
using VectorSearch.Application.Queries.SearchDocuments;

namespace VectorSearch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VectorSearchController : ControllerBase
{
    private readonly IMediator _mediator;

    public VectorSearchController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("index")]
    [ProducesResponseType(typeof(IndexDocumentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> IndexDocument(
        [FromBody] IndexDocumentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new IndexDocumentCommand(request.Title, request.Content);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
    
    [HttpPost("search")]
    [ProducesResponseType(typeof(IReadOnlyList<SearchResultDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromBody] SearchRequest request,
        CancellationToken cancellationToken)
    {
        var query = new SearchDocumentsQuery(request.QueryText, request.Limit);
        var results = await _mediator.Send(query, cancellationToken);
        return Ok(results);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(IndexDocumentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocument(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetDocumentQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}