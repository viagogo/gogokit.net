### New in 0.10.1 (Released 2016/02/20)
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