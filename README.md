# Vector Search Engine

Semantic document search engine built with .NET 10, Qdrant VectorDB, and Gemini Embeddings. Unlike keyword search, this system understands the **meaning** of your query and finds the most relevant documents.

## How It Works

```
Document Input
    → Split into chunks (500 words, 50 word overlap)
    → Each chunk sent to Gemini Embedding API
    → Gemini converts each chunk to a 3072-dimensional vector
    → Vectors stored in Qdrant with metadata

Search Query
    → Query text sent to Gemini Embedding API
    → Query converted to a vector
    → Qdrant finds nearest vectors (Cosine Similarity)
    → Results returned with similarity scores
```

## Architecture

Clean Architecture with 4 layers:

```
vector-search-engine/
├── docker-compose.yml
├── .env
└── src/
    ├── VectorSearch.Domain/
    │   ├── Entities/          # Document, DocumentChunk, SearchResult
    │   └── Interfaces/        # IEmbeddingService, IVectorRepository, IDocumentRepository
    ├── VectorSearch.Application/
    │   ├── Commands/          # IndexDocumentCommand
    │   ├── Queries/           # SearchDocumentsQuery, GetDocumentQuery
    │   ├── Services/          # ChunkingService
    │   └── DTOs/              # IndexDocumentDto, SearchResultDto
    ├── VectorSearch.Infrastructure/
    │   ├── Services/          # GeminiEmbeddingService
    │   ├── Repositories/      # QdrantVectorRepository, InMemoryDocumentRepository
    │   └── Models/            # GeminiEmbeddingResponse
    └── VectorSearch.API/
        ├── Controllers/       # VectorSearchController
        └── Middleware/        # ExceptionHandlingMiddleware
```

## Tech Stack

| Component | Technology |
|---|---|
| API | .NET 10 Web API |
| Vector Database | Qdrant |
| Embedding Model | Gemini Embedding 001 (3072 dimensions) |
| Mediator | MediatR 11.x |
| Containerization | Docker + Docker Compose |
| Protocol | gRPC (Qdrant), REST (Gemini API) |

## Key Concepts

**Embeddings** — Text converted to a vector (array of floats). Semantically similar texts produce similar vectors. "Machine learning" and "ML algorithms" are different words but produce nearby vectors.

**Chunking** — Long documents are split into smaller pieces. Smaller chunks produce more precise search results. Overlap (50 words) preserves context at chunk boundaries.

**Cosine Similarity** — Measures the angle between two vectors. Score close to 1.0 = very similar, close to 0.0 = unrelated. Used to rank search results.

**gRPC** — Binary protocol used to communicate with Qdrant. Much faster than REST/JSON for large vector transfers.

## Getting Started

### Prerequisites

- Docker & Docker Compose
- Gemini API Key ([Get one here](https://aistudio.google.com/))

### Setup

1. Clone the repository:
```bash
git clone https://github.com/yourusername/vector-search-engine
cd vector-search-engine
```

2. Create `.env` file in the root directory:
```
GEMINI_API_KEY=your_gemini_api_key_here
```

3. Start the services:
```bash
docker-compose up --build
```

4. Open Swagger UI:
```
http://localhost:5000/swagger
```

## API Endpoints

### Index a Document
```http
POST /api/vectorsearch/index
Content-Type: application/json

{
  "title": "Artificial Intelligence Overview",
  "content": "Artificial intelligence is the simulation of human intelligence..."
}
```

Response:
```json
{
  "documentId": "50fcc4b2-74e1-4857-befb-828b387516aa",
  "title": "Artificial Intelligence Overview",
  "chunkCount": 1,
  "status": "Completed"
}
```

### Search Documents
```http
POST /api/vectorsearch/search
Content-Type: application/json

{
  "queryText": "What is machine learning?",
  "limit": 5
}
```

Response:
```json
[
  {
    "documentId": "50fcc4b2-74e1-4857-befb-828b387516aa",
    "documentTitle": "Artificial Intelligence Overview",
    "content": "Artificial intelligence is the simulation...",
    "score": 0.64,
    "chunkIndex": 0
  }
]
```

### Get Document by ID
```http
GET /api/vectorsearch/{id}
```

## Semantic Search vs Keyword Search

| Query | Keyword Search | Semantic Search |
|---|---|---|
| "What is ML?" | Finds "ML" literally | Finds "machine learning" content |
| "How to cook pasta?" | No results in AI docs | Low score (0.44) — correctly irrelevant |
| "Neural networks" | Only exact matches | Finds "deep learning" content too |

## Services

| Service | Port | Description |
|---|---|---|
| Vector Search API | 5000 | .NET Web API + Swagger |
| Qdrant Dashboard | 6333 | VectorDB web UI |
| Qdrant gRPC | 6334 | Used internally by the API |

Visit `http://localhost:6333/dashboard` to explore stored vectors visually.

## Project Structure Decisions

**Why InMemory for document repository?** This project focuses on VectorDB concepts. A real database will be introduced in a later project.

**Why no Python service?** Unlike Project 1 (NLP with spaCy), embeddings are handled entirely by Gemini API — no need for a separate Python service.

**Why Qdrant volumes?** Vector data persists across container restarts. Without a volume, all indexed documents would be lost on restart.