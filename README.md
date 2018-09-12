# GogoKit - viagogo API Client Library for .NET

[![NuGet version](https://badge.fury.io/nu/gogokit.svg)][nuget]
[![Build status](https://ci.appveyor.com/api/projects/status/ri2rbvoinudw27en/branch/master?svg=true)][appveyor]

[appveyor]: https://ci.appveyor.com/project/viagogo/gogokit-net/branch/master
[nuget]: http://www.nuget.org/packages/GogoKit

GogoKit is a lightweight, async viagogo API client library for .NET. Our [developer site][apidocs]
documents all of the viagogo APIs.

[apidocs]: http://developer.viagogo.net


## Getting Started

GogoKit is available on NuGet.

```
Install-Package GogoKit
```


## Usage

```c#
// All methods require OAuth2 authentication. To get OAuth2 credentials for your
// application, see http://developer.viagogo.net/#authentication.
var client = new ViagogoClient(new ProductHeaderValue("AwesomeApp", "1.0"),
                               CLIENT_ID,
                               CLIENT_SECRET);

// Get an access token. See http://developer.viagogo.net/#getting-access-tokens
var token = await client.OAuth2.GetClientAccessTokenAsync(/*List of scopes*/ new string[] {});
await client.TokenStore.SetTokenAsync(token);

// Get a list of events, categories, venues and metro areas that match the given
// search query
var searchResults = await client.Search.GetSearchResultsAsync("FC Barcelona tickets");

// Get the different event genres (see http://developer.viagogo.net/#entities)
var genres = await client.Categories.GetAllGenresAsync();
```

### Sandbox Environment

```c#
// You can use the GogoKitConfiguration to switch between the sandbox and
// production environments. See http://developer.viagogo.net/#sandbox-environment
var client = new ViagogoClient (new ProductHeaderValue("AwesomeApp", "1.0"),
                                new GogoKitConfiguration(SANDBOX_CLIENT_ID, SANDBOX_CLIENT_SECRET)
                                {
                                    ViagogoApiEnvironment = ApiEnvironment.Sandbox
                                });
```


## Supported Platforms

* .NET Core 2.0
* .NET Framework 4.6.1
* Mono 5.4
* Xamarin.iOS 10.14
* Xamarin.Mac 3.8
* Xamarin.Android 7.5
* Universal Windows Platform vNext


## How to contribute

All submissions are welcome. Fork the repository, read the rest of this README
file and make some changes. Once you're done with your changes send a pull
request. Thanks!


## Need Help? Found a bug?

[submitanissue]: https://github.com/viagogo/gogokit.net/issues

Just [submit a issue][submitanissue] if you need any help. And, of course, feel
free to submit pull requests with bug fixes or changes.
