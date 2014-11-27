using System.Collections.Generic;
using System.Runtime.Serialization;
using Viagogo.Sdk.Json;

namespace Viagogo.Sdk.Resources
{
    [DataContract]
    public class PagedResource<T> : Resource where T : Resource
    {
        [DataMember(Name = "total_items")]
        public int TotalItems { get; set; }

        [DataMember(Name = "page")]
        public int Page { get; set; }

        [DataMember(Name = "page_size")]
        public int PageSize { get; set; }

        [Embedded("items")]
        public IReadOnlyList<T> Items { get; set; }
    }
}
