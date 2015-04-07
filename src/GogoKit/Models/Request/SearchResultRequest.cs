using System;
using System.Collections.Generic;
using System.Linq;

namespace GogoKit.Models.Request
{
    public class SearchResultRequest : RequestParameters<SearchResultEmbed, string>
    {
        public IEnumerable<SearchResultTypeFilter> Type
        {
            get
            {
                string typeText;
                if (!Parameters.TryGetValue("type", out typeText) || typeText == null)
                {
                    return null;
                }

                return typeText.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                               .Select(t => (SearchResultTypeFilter) Enum.Parse(typeof(SearchResultTypeFilter), t, true));
            }
            set
            {
                SetParameter("type", string.Join(",", value ?? new SearchResultTypeFilter[] {}));
            }
        }

        protected override IDictionary<SearchResultEmbed, string> EmbedFieldNameMap
        {
            get
            {
                return new Dictionary<SearchResultEmbed, string>
                {
                    {SearchResultEmbed.Category, "category"},
                    {SearchResultEmbed.Event, "event"},
                    {SearchResultEmbed.Genre, "genre"},
                    {SearchResultEmbed.MetroArea, "metro_area"},
                    {SearchResultEmbed.Venue, "venue"},
                };
            }
        }
    }

    public enum SearchResultTypeFilter
    {
        Category,
        Event,
        Venue,
        MetroArea
    }

    public enum SearchResultEmbed
    {
        Category,
        Event,
        Genre,
        MetroArea,
        Venue,
    }
}
