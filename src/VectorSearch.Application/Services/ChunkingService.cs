namespace VectorSearch.Application.Services;

public class ChunkingService
{
    private readonly int _chunkSize;
    private readonly int _overlapSzie;

    public ChunkingService(int chunkSize = 500, int overlapSzie = 50)
    {
        _chunkSize = chunkSize;
        _overlapSzie = overlapSzie;
    }

    public IReadOnlyList<string> Chunk(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return [];

        var chunks = new List<string>();
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var currentChunk = new List<string>();
        var wordCount = 0;

        foreach (var word in words)
        {
            currentChunk.Add(word);
            wordCount++;

            if (wordCount >= _chunkSize)
            {
                chunks.Add(string.Join(" ", currentChunk));
                
                var overlapWords = currentChunk.TakeLast(_overlapSzie).ToList();
                
                currentChunk = overlapWords;
                wordCount = overlapWords.Count;
            }
        }
        
        if(currentChunk.Count > 0)
            chunks.Add(string.Join(" ", currentChunk));

        return chunks;
    }
}