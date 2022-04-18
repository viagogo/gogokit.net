using System;
using System.Collections.Generic;
using System.Text;

namespace GogoKit.Models.Request
{
    public class CatalogEventRequest : RequestParameters<CatalogEventEmbed, CatalogEventSort>
    {
        protected override IDictionary<CatalogEventSort, string> SortFieldNameMap =>
            new Dictionary<CatalogEventSort, string>
            {
                {CatalogEventSort.ResourceVersion, "resource_version" }
            };
    }

    public enum CatalogEventEmbed
    {
    }

    public enum CatalogEventSort
    {
        ResourceVersion
    }
}
