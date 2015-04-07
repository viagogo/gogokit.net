using System;
using System.Collections.Generic;

namespace GogoKit.Models.Request
{
    public class CategoryRequest : RequestParameters<CategoryEmbed, CategorySort>
    {
        protected override IDictionary<CategoryEmbed, string> EmbedFieldNameMap
        {
            get
            {
                return new Dictionary<CategoryEmbed, string>
                {
                    {CategoryEmbed.TopChildren, "top_children"},
                    {CategoryEmbed.TopEvents, "top_events"},
                    {CategoryEmbed.TopPerformers, "top_performers"},
                };
            }
        }

        protected override IDictionary<CategorySort, string> SortFieldNameMap
        {
            get
            {
                return new Dictionary<CategorySort, string>
                {
                    {CategorySort.Id, "id"},
                    {CategorySort.Name, "name"},
                    {CategorySort.MinEventDate, "min_event_date"},
                    {CategorySort.MaxEventDate, "max_event_date"},
                    {CategorySort.MinTicketPrice, "min_ticket_price"},
                };
            }
        }

        public DateTimeOffset? MinStartDate
        {
            get
            {
                DateTimeOffset minStartDate;
                string minStartDateText;
                if (!Parameters.TryGetValue("min_start_date", out minStartDateText) ||
                    !DateTimeOffset.TryParse(minStartDateText, out minStartDate))
                {
                    return null;
                }

                return minStartDate;
            }
            set
            {
                Parameters.Add("min_start_date", value != null ? value.ToString() : null);
            }
        }

        public DateTimeOffset? MaxStartDate
        {
            get
            {
                DateTimeOffset maxStartDate;
                string maxStartDateText;
                if (!Parameters.TryGetValue("max_start_date", out maxStartDateText) ||
                    !DateTimeOffset.TryParse(maxStartDateText, out maxStartDate))
                {
                    return null;
                }

                return maxStartDate;
            }
            set
            {
                Parameters.Add("max_start_date", value != null ? value.ToString() : null);
            }
        }

        public decimal? MaxTicketPrice
        {
            get
            {

                decimal maxTicketPrice;
                string maxTicketPriceText;
                if (!Parameters.TryGetValue("max_ticket_price", out maxTicketPriceText) ||
                    !decimal.TryParse(maxTicketPriceText, out maxTicketPrice))
                {
                    return null;
                }

                return maxTicketPrice;
            }
            set
            {
                Parameters.Add("max_ticket_price", value != null ? value.ToString() : null);
            }
        }

        public bool? OnlyWithEvents
        {
            get
            {

                bool onlyWithEvents;
                string onlyWithEventsText;
                if (!Parameters.TryGetValue("only_with_events", out onlyWithEventsText) ||
                    !bool.TryParse(onlyWithEventsText, out onlyWithEvents))
                {
                    return null;
                }

                return onlyWithEvents;
            }
            set
            {
                Parameters.Add("only_with_events", value != null ? value.ToString() : null);
            }
        }

        public bool? OnlyWithTickets
        {
            get
            {

                bool onlyWithTickets;
                string onlyWithTicketsText;
                if (!Parameters.TryGetValue("only_with_tickets", out onlyWithTicketsText) ||
                    !bool.TryParse(onlyWithTicketsText, out onlyWithTickets))
                {
                    return null;
                }

                return onlyWithTickets;
            }
            set
            {
                Parameters.Add("only_with_tickets", value != null ? value.ToString() : null);
            }
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
