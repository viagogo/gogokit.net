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
