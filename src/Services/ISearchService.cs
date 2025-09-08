using TomAndJerry.Model;

namespace TomAndJerry.Services;

public interface ISearchService
{
    Task<IEnumerable<Video>> SearchAsync(string searchTerm);
    Task<IEnumerable<string>> GetSearchSuggestionsAsync(string partialTerm);
    event Action<IEnumerable<Video>>? OnSearchResultsChanged;
}
