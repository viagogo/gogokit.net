using System.Collections.Generic;

namespace GogoKit.Models.Request
{

    public class CatalogVenueRequest : RequestParameters<CatalogVenueEmbed, CatalogVenueSort>
    {
        protected override IDictionary<CatalogVenueSort, string> SortFieldNameMap =>
            new Dictionary<CatalogVenueSort, string>
            {
                {CatalogVenueSort.ResourceVersion, "resource_version" }
            };
    }

    public enum CatalogVenueEmbed
    {
    }

    public enum CatalogVenueSort
    {
        ResourceVersion
    }
}
