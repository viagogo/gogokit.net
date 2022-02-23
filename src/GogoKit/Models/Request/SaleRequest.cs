using System.Collections.Generic;

namespace GogoKit.Models.Request
{
    public class SaleRequest : RequestParameters<SaleEmbed, SaleSort>
    {
        protected override IDictionary<SaleSort, string> SortFieldNameMap =>
            new Dictionary<SaleSort, string>
            {
                {SaleSort.CreatedAt, "created_at"},
                {SaleSort.ResourceVersion, "resource_version"}
            };


        public string StatusFilter
        {
            get
            {
                if (!Parameters.TryGetValue("sale_statuses", out var valueText))
                {
                    return null;
                }

                return valueText;
            }
            set => SetParameter("sale_statuses", value);
        }

        public string ExternalListingIdFilter
        {
            get
            {
                if (!Parameters.TryGetValue("external_listing_id", out var valueText))
                {
                    return null;
                }

                return valueText;
            }
            set => SetParameter("external_listing_id", value);
        }
    }

    public enum SaleEmbed
    {
    }

    public enum SaleSort
    {
        CreatedAt,
        ResourceVersion
    }
}
