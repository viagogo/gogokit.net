using System.Collections.Generic;

namespace GogoKit.Models.Request
{
    public class CurrencyRequest : RequestParameters<string, CurrencySort>
    {
        protected override IDictionary<CurrencySort, string> SortFieldNameMap =>
            new Dictionary<CurrencySort, string>
            {
                {CurrencySort.Code, "code"},
                {CurrencySort.Name, "name"}
            };
    }

    public enum CurrencySort
    {
        Code,
        Name
    }
}
