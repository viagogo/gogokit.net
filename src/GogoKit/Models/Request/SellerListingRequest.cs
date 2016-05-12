using System.Collections.Generic;

namespace GogoKit.Models.Request
{
    public class SellerListingRequest : RequestParameters<SellerListingEmbed, SellerListingSort>
    {
        protected override IDictionary<SellerListingSort, string> SortFieldNameMap =>
            new Dictionary<SellerListingSort, string>
            {
                {SellerListingSort.CreatedAt, "created_at" },
                {SellerListingSort.TicketPrice, "ticket_price" }
            };
    }

    public enum SellerListingEmbed
    {
    }

    public enum SellerListingSort
    {
        CreatedAt,
        TicketPrice,
        ResourceVersion
    }
}
