using System.Collections.Generic;

namespace GogoKit.Models.Request
{
    public class WebhookRequest : RequestParameters<WebhookEmbed, WebhookSort>
    {
        protected override IDictionary<WebhookSort, string> SortFieldNameMap =>
            new Dictionary<WebhookSort, string>
            {
                {WebhookSort.CreatedAt, "created_at"}
            };
    }

    public enum WebhookEmbed
    {
    }

    public enum WebhookSort
    {
        CreatedAt
    }
}
