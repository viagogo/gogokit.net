using System;
using System.Collections.Generic;

namespace GogoKit.Models.Request
{
    public class CategoryRequest : RequestParameters<CategoryEmbed, CategorySort>
    {
        protected override IDictionary<CategoryEmbed, string> EmbedFieldNameMap =>
            new Dictionary<CategoryEmbed, string>
            {
                {CategoryEmbed.TopChildren, "top_children"},
                {CategoryEmbed.TopEvents, "top_events"},
                {CategoryEmbed.TopPerformers, "top_performers"},
            };

        protected override IDictionary<CategorySort, string> SortFieldNameMap =>
            new Dictionary<CategorySort, string>
            {
                {CategorySort.Id, "id"},
                {CategorySort.Name, "name"},
                {CategorySort.MinEventDate, "min_event_date"},
                {CategorySort.MaxEventDate, "max_event_date"},
                {CategorySort.MinTicketPrice, "min_ticket_price"},
            };

        public DateTimeOffset? MinStartDate
        {
            get { return GetParameter("min_start_date", DateTimeOffset.Parse); }
            set { SetParameter("min_start_date", value); }
        }

        public DateTimeOffset? MaxStartDate
        {
            get { return GetParameter("max_start_date", DateTimeOffset.Parse); }
            set { SetParameter("max_start_date", value); }
        }

        public decimal? MaxTicketPrice
        {
            get { return GetParameter("max_ticket_price", decimal.Parse); }
            set { SetParameter("max_ticket_price", value); }
        }

        public bool? OnlyWithEvents
        {
            get { return GetParameter("only_with_events", bool.Parse); }
            set { SetParameter("only_with_events", value); }
        }

        public bool? OnlyWithTickets
        {
            get { return GetParameter("only_with_tickets", bool.Parse); }
            set { SetParameter("only_with_tickets", value); }
        }
    }

    public enum CategoryEmbed
    {
        TopChildren,
        TopEvents,
        TopPerformers
    }

    public enum CategorySort
    {
        Id,
        Name,
        MinEventDate,
        MaxEventDate,
        MinTicketPrice
    }
}
