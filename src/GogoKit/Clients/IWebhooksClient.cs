using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using HalKit.Http;

namespace GogoKit.Clients
{
    public interface IWebhooksClient
    {
        Task<Webhook> GetAsync(int webhookId);
        Task<Webhook> GetAsync(int webhookId, WebhookRequest request);

        Task<PagedResource<Webhook>> GetAsync();

        Task<PagedResource<Webhook>> GetAsync(WebhookRequest request);

        Task<IReadOnlyList<Webhook>> GetAllAsync();

        Task<IReadOnlyList<Webhook>> GetAllAsync(WebhookRequest request);

        Task<Webhook> CreateAsync(NewWebhook webhook);

        Task<Webhook> CreateAsync(NewWebhook webhook, WebhookRequest request);

        Task<Webhook> UpdateAsync(int webhookId, WebhookUpdate webhookUpdate);

        Task<Webhook> UpdateAsync(int webhookId, WebhookUpdate webhookUpdate, WebhookRequest request);

        Task<IApiResponse> DeleteAsync(int webhookId);
    }
}
