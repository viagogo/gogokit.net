using System.Collections.Generic;

namespace GogoKit.Models.Request
{
    public class CountryRequest : RequestParameters<string, CountrySort>
    {
        protected override IDictionary<CountrySort, string> SortFieldNameMap
        {
            get
            {
                return new Dictionary<CountrySort, string>
                {
                    {CountrySort.Code, "code"},
                    {CountrySort.Name, "name"}
                };
            }
        }
    }

    public enum CountrySort
    {
        Code,
        Name
    }
}
