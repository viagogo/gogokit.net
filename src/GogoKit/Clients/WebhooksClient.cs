using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using GogoKit.Services;
using HalKit;
using HalKit.Http;
using System.Net.Http;
using System.Threading;
using HalKit.Services;

namespace GogoKit.Clients
{
    public class WebhooksClient : IWebhooksClient
    {
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;
        private readonly ILinkResolver _linkResolver;

        public WebhooksClient(
            IHalClient halClient,
            ILinkFactory linkFactory)
        {
            _halClient = halClient;
            _linkFactory = linkFactory;
            _linkResolver = new LinkResolver();
        }

        public Task<Webhook> GetAsync(int webhookId)
        {
            return GetAsync(webhookId, new WebhookRequest());
        }

        public async Task<Webhook> GetAsync(int webhookId, WebhookRequest request)
        {
            var webhookLink = await _linkFactory.CreateLinkAsync($"webhooks/{webhookId}").ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Webhook>(webhookLink, request).ConfigureAwait(_halClient);
        }

        public Task<Webhooks> GetAsync()
        {
            return GetAsync(new WebhookRequest());
        }

        public async Task<Webhooks> GetAsync(WebhookRequest request)
        {
            var webhookLink = await _linkFactory.CreateLinkAsync($"webhooks").ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Webhooks>(webhookLink, request).ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<Webhook>> GetAllAsync()
        {
            return GetAllAsync(new WebhookRequest());
        }

        public async Task<IReadOnlyList<Webhook>> GetAllAsync(WebhookRequest request)
        {
            var webhookLink = await _linkFactory.CreateLinkAsync($"webhooks").ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Webhook>(webhookLink, request).ConfigureAwait(_halClient);
        }

        public Task<Webhook> CreateAsync(NewWebhook webhook)
        {
            return CreateAsync(webhook, new WebhookRequest());
        }

        public async Task<Webhook> CreateAsync(NewWebhook webhook, WebhookRequest request)
        {
            var createLink = await _linkFactory.CreateLinkAsync("webhooks").ConfigureAwait(_halClient);
            return await _halClient.PostAsync<Webhook>(createLink, webhook, request).ConfigureAwait(_halClient);
        }

        public Task<Webhook> UpdateAsync(int webhookId, WebhookUpdate webhookUpdate)
        {
            return UpdateAsync(webhookId, webhookUpdate, new WebhookRequest());
        }

        public async Task<Webhook> UpdateAsync(int webhookId, WebhookUpdate webhookUpdate, WebhookRequest request)
        {
            var updateLink = await _linkFactory.CreateLinkAsync($"webhooks/{webhookId}").ConfigureAwait(_halClient);
            return await _halClient.PostAsync<Webhook>(updateLink, webhookUpdate, request).ConfigureAwait(_halClient);
        }

        public async Task<IApiResponse> DeleteAsync(int webhookId)
        {
            var deleteLink = await _linkFactory.CreateLinkAsync($"webhooks/{webhookId}").ConfigureAwait(_halClient);
            return await _halClient.DeleteAsync(deleteLink).ConfigureAwait(_halClient);
        }

        public async Task<IApiResponse> PingAsync(int webhookId)
        {
            var pingLink = await _linkFactory.CreateLinkAsync($"webhooks/{webhookId}/ping").ConfigureAwait(_halClient);
            var pingUrl = _linkResolver.ResolveLink(pingLink, new Dictionary<string, string>());
            var response = await _halClient.HttpConnection.SendRequestAsync<string>(
                                    pingUrl,
                                    HttpMethod.Post,
                                    null,
                                    new Dictionary<string, IEnumerable<string>>(),
                                    CancellationToken.None);
            return response;
        }
    }
}