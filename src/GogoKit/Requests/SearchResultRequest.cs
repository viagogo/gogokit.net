using System;

namespace GogoKit.Requests
{
    public class SearchResultRequest : PageRequest
    {
        public SearchResultTypeFilter? TypeFilter { get; set; }
    }

    [Flags]
    public enum SearchResultTypeFilter
    {
        Category = 1,
        Event = 2,
        Venue = 4,
        MetroArea = 8
    }
}
