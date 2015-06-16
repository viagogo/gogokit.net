using System;
using System.Collections.Generic;
using System.Linq;

namespace GogoKit.Models.Request
{
    public class EventRequest : RequestParameters<EventEmbed, EventSort>
    {
        protected override IDictionary<EventEmbed, string> EmbedFieldNameMap
        {
            get
            {
                return new Dictionary<EventEmbed, string>
                {
                    {EventEmbed.Category, "category"}
                };
            }
        }

        protected override IDictionary<EventSort, string> SortFieldNameMap
        {
            get
            {
                return new Dictionary<EventSort, string>
                {
                    {EventSort.Id, "id"},
                    {EventSort.Name, "name"},
                    {EventSort.StartDate, "start_date"},
                    {EventSort.MinTicketPrice, "min_ticket_price"},
                    {EventSort.Distance, "distance"}
                };
            }
        }

        public double? Latitude
        {
            get { return GetParameter("latitude", double.Parse); }
            set { SetParameter("latitude", value); }
        }

        public double? Longitude
        {
            get { return GetParameter("longitude", double.Parse); }
            set { SetParameter("longitude", value); }
        }

        public int? MaxDistanceInMetres
        {
            get { return GetParameter("max_distance", int.Parse); }
            set { SetParameter("max_distance", value); }
        }

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

        public bool? OnlyWithTickets
        {
            get { return GetParameter("only_with_tickets", bool.Parse); }
            set { SetParameter("only_with_tickets", value); }
        }

        public EventTimeFrame? TimeFrame
        {
            get { return GetParameter("time_frame", t => (EventTimeFrame) Enum.Parse(typeof(EventTimeFrame), t, true)); }
            set { SetParameter("time_frame", value); }
        }

        public IEnumerable<DayOfWeek> Days
        {
            get
            {
                string days;
                if (!Parameters.TryGetValue("days", out days) || days == null)
                {
                    return new DayOfWeek[] {};
                }

                return days.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                           .Select(d => (DayOfWeek) Enum.Parse(typeof(DayOfWeek), d));
            }
            set
            {
                var daysValues = value != null
                                    ? string.Join(",", value.Select(d => d.ToString()))
                                    : null;
                SetParameter("days", daysValues);
            }
        }
    }

    public enum EventEmbed
    {
        Category
    }

    public enum EventSort
    {
        Id,
        Name,
        StartDate,
        MinTicketPrice,
        Distance
    }

    public enum EventTimeFrame
    {
        Today,
        Tomorrow,
        ThisWeekend,
        ThisWeek,
        NextWeek,
        ThisMonth,
        NextMonth
    }
}
