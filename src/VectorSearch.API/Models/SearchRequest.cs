namespace VectorSearch.API.Models;

public class SearchRequest
{
    public string QueryText { get; init; } = string.Empty;
    public int Limit { get; init; } = 5;
}