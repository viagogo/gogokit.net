using System.Collections.Generic;

namespace GogoKit.Models.Request
{
    public class ListingRequest : RequestParameters<string, ListingSort>
    {
        protected override IDictionary<ListingSort, string> SortFieldNameMap
        {
            get
            {
                return new Dictionary<ListingSort, string>
                {
                    {ListingSort.Id, "id"},
                    {ListingSort.TicketPrice, "ticket_price"},
                    {ListingSort.SeatingSection, "seating.section"},
                    {ListingSort.NumberOfTickets, "number_of_tickets"},
                };
            }
        }

        public int? NumberOfTickets
        {
            get { return GetParameter("number_tickets", int.Parse); }
            set { SetParameter("number_of_tickets", value); }
        }
    }

    public enum ListingSort
    {
        Id,
        TicketPrice,
        SeatingSection,
        NumberOfTickets
    }
}
