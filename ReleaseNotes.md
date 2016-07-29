### New in 0.27.0 (Released (2016/07/29)
* Added new ViagogoClient constructor that allows customisation of the entire stack

### New in 0.26.0 (Released (2016/07/06)
* Allow HalKit.Json.IJsonSerializer injection into ViagogoClient

### New in 0.25.1 (Released (2016/06/30)
* Rename InHandDate to InHandAt to match response field name

### New in 0.25.0 (Released (2016/06/27)
* Add IsInstantDelivery property to seller listings

### New in 0.24.1 (Released 2016/06/16)
* Filter duplicates on get all seller listings

### New in 0.24.0 (Released 2016/06/14)
* Added Notes property on NewSellerListing request

### New in 0.23.1 (Released 2016/06/02)
Fixed bug in SellerListingClient.GetConstraintsForEventAsync

### New in 0.23.0 (Released 2016/06/02)
* Added ISellerListingClient method for getting SellerListingPurchasePreviews for requested events

### New in 0.22.0 (Released 2016/05/19)
* Added DisplaySeating property on SellerListing response

### New in 0.21.1 (Released 2016/05/12)
* Fixed ErrorHandler to populate ApiError property in ApiErrorExceptions

### New in 0.21.0 (Released 2016/05/06)
* Add new properties and links on SellerListing
* Added ISellerListingClient overload for getting constraints for requested events
* Modified BearerTokenAuthenticationHandler to not delete tokens if there is a race between requests refreshing a token

### New in 0.20.1 (Released 2016/05/04)
* Fixed bug in logic for getting changed resources

### New in 0.20.0 (Released 2016/05/03)
* Added missing links on SellerListing
* Added missing links of SearchResult

### New in 0.19.1 (Released 2016/05/02)
* Fixed issue where we weren't figuring out the last page correctly when getting changed resources

### New in 0.19.0 (Released 2016/05/02)
* Added ISellerListingClient methods for getting listing changes
* Add extension method for getting resources that have changed since some previous request

### New in 0.18.0 (Released 2016/04/10)
* Added TicketProceeds property to SellerListing request model

### New in 0.17.0 (Released 2016/04/10)
* Added InHandDate property to SellerListing response and request models

### New in 0.16.1 (Released 2016/03/29)
* Fixed issue where Webhook.Id property was not getting deserialized correctly

### New in 0.16.0 (Released 2016/03/17)
* Added StateProvince property on EmbeddedVenue

### New in 0.15.0 (Released 2016/03/08)
* Added IWebhooksClient method for pinging webhooks
* Added models for webhook payloads

### New in 0.14.0 (Released 2016/03/03)
* Add DateConfirmed and OnSaleDate properties to EmbeddedEvent

### New in 0.13.0 (Released 2016/03/01)
* Added IShipmentsClient and functionality for creating shipments and pickups

### New in 0.12.1 (Released 2016/02/26)
* Stop using IUserClient in SellerListingsClient to reduce scope requirements

### New in 0.12.0 (Released 2016/02/23)
* Added ISellerListingsClient method for creating requested event listings

### New in 0.11.0 (Released 2016/02/21)
* Added ISalesClient functionality for managing sales and e-tickets

### New in 0.10.1 (Released 2016/02/21)
* Added IOAuth2Client methods for the Authorization Code grant type
* Pass `redirect_uri` parameter when getting tokens via auth code grant type

### New in 0.10.0 (Released 2016/02/20)
* Added IOAuth2Client methods for the Authorization Code grant type

### New in 0.9.0 (Released 2016/02/13)
* Added support for cancelling requests in the ISellerListingClient, IBatchClient and IUserClient

### New in 0.8.2 (Released 2016/02/10)
* Fixed BatchClient issue generating the Uri for the batch endpoint

### New in 0.8.1 (Released 2016/02/10)
* Fixed BatchClient issue generating the Uri for the batch endpoint

### New in 0.8.0 (Released 2016/02/08)
* Add `external_id` and `notes` to create seller listing payloads

### New in 0.7.1 (Released 2016/02/05)
* Add BatchClient to IViagogoClient

### New in 0.7.0 (Released 2016/02/04)
* ApiException.Message now returns actual error information from the API
* Add new request event seller listing request

### New in 0.6.0 (Released 2015/12/15)
* ApiException.Message now returns actual error information from the API
* `purchasepreview:confirm` link added to PurchasePreview resource

### New in 0.5.0 (Released 2015/11/23)
* BearerAuthenticationHandler gets a `client_credentials` token automatically if
the token store doesn't have a token yet

### New in 0.4.0 (Released 2015/06/16)
* Added WebhooksClient

### New in 0.3.0 (Released 2015/06/16)
* Added SellerListingsClient

### New in 0.2.0 (Released 2015/06/11)
* Updated Resource models to define Link properties using HalKit's new RelAttribute

### New in 0.1.2 (Released 2015/04/30)
* Fixed a bug in ViagogoClient where it wasn't creating the LinkFactory correctly

### New in 0.1.1 (Relesed 2015/04/15)
* Fixed bug in OAuth2Client where it wasn't refreshing access tokens correctly

### New in 0.1.0 (Released 2015/04/07)
* Initial release
