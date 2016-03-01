using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GogoKit.Models.Request;
using GogoKit.Models.Response;
using HalKit;
using GogoKit.Services;

namespace GogoKit.Clients
{
    public class ShipmentsClient : IShipmentsClient
    {
        private readonly IHalClient _halClient;
        private readonly ILinkFactory _linkFactory;

        public ShipmentsClient(IHalClient halClient, ILinkFactory linkFactory)
        {
            _halClient = halClient;
            _linkFactory = linkFactory;
        }

        public Task<Shipment> CreateAsync(int saleId)
        {
            return CreateAsync(saleId, new ShipmentRequest());
        }

        public Task<Shipment> CreateAsync(int saleId, ShipmentRequest request)
        {
            return CreateAsync(saleId, request, CancellationToken.None);
        }

        public async Task<Shipment> CreateAsync(int saleId, ShipmentRequest request, CancellationToken cancellationToken)
        {
            var createShipmentLink = await _linkFactory.CreateLinkAsync($"sales/{saleId}/shipments").ConfigureAwait(_halClient);
            return await _halClient.PutAsync<Shipment>(createShipmentLink, null, request, cancellationToken)
                                   .ConfigureAwait(_halClient);
        }

        public Task<IReadOnlyList<Shipment>> GetAllAsync(int saleId)
        {
            return GetAllAsync(saleId, new ShipmentRequest());
        }

        public Task<IReadOnlyList<Shipment>> GetAllAsync(int saleId, ShipmentRequest request)
        {
            return GetAllAsync(saleId, request, CancellationToken.None);
        }

        public async Task<IReadOnlyList<Shipment>> GetAllAsync(
            int saleId,
            ShipmentRequest request,
            CancellationToken cancellationToken)
        {
            var shipmentsLink = await _linkFactory.CreateLinkAsync($"sales/{saleId}/shipments").ConfigureAwait(_halClient);
            return await _halClient.GetAllPagesAsync<Shipment>(shipmentsLink, request, cancellationToken).ConfigureAwait(_halClient);
        }

        public Task<Shipments> GetAsync(int saleId)
        {
            return GetAsync(saleId, new ShipmentRequest());
        }

        public async Task<Shipments> GetAsync(int saleId, ShipmentRequest request)
        {
            var shipmentsLink = await _linkFactory.CreateLinkAsync($"sales/{saleId}/shipments").ConfigureAwait(_halClient);
            return await _halClient.GetAsync<Shipments>(shipmentsLink, request).ConfigureAwait(_halClient);
        }
    }
}
