using MediatR;
using VectorSearch.Application.DTOs;
using VectorSearch.Application.Services;
using VectorSearch.Domain.Entities;
using VectorSearch.Domain.Interfaces;

namespace VectorSearch.Application.Commands.IndexDocument;

public class IndexDocumentCommandHandler : IRequestHandler<IndexDocumentCommand, IndexDocumentDto>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IVectorRepository _vectorRepository;
    private readonly IEmbeddingService _embeddingService;
    private readonly ChunkingService _chunkingService;

    public IndexDocumentCommandHandler(
        IDocumentRepository documentRepository,
        IVectorRepository vectorRepository,
        IEmbeddingService embeddingService,
        ChunkingService chunkingService)
    {
        _documentRepository = documentRepository;
        _vectorRepository = vectorRepository;
        _embeddingService = embeddingService;
        _chunkingService = chunkingService;
    }

    public async Task<IndexDocumentDto> Handle(IndexDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = new Document
        {
            Title = request.Title,
            Content = request.Content
        };
        
        await _documentRepository.AddAsync(document, cancellationToken);
        document.MarkAsProcessing();

        try
        {
            var chunkTexts = _chunkingService.Chunk(request.Content);

            var embeddings = await _embeddingService.GenerateEmbeddingAsync(chunkTexts, cancellationToken);
            
            var chunks = chunkTexts
                .Select((text, index) =>
                {
                    var chunk = new DocumentChunk
                    {
                        DocumentId = document.Id,
                        Content = text,
                        ChunkIndex = index,
                        StartPosition = index * 500
                    };
                    chunk.SetEmbedding(embeddings[index]);
                    return chunk;
                })
                .ToList();
            
            await _vectorRepository.UpsertChunkAsync(chunks, cancellationToken);
            
            document.MarkAsCompleted(chunks.Count);
            await _documentRepository.UpdateAsync(document, cancellationToken);

            return new IndexDocumentDto
            {
                DocumentId = document.Id,
                Title = document.Title,
                ChunkCount = chunks.Count,
                Status = document.Status.ToString()
            };
        }
        catch
        {
            document.MarkAsFailed();
            await _documentRepository.UpdateAsync(document, cancellationToken);
            throw;
        }
        
    }
}