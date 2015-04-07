# GogoKit - viagogo API Client Library for .NET

[![Build status](https://ci.appveyor.com/api/projects/status/ri2rbvoinudw27en/branch/master?svg=true)][appveyor]

[appveyor]: https://ci.appveyor.com/project/viagogo/gogokit-net/branch/master

GogoKit is a lightweight, async viagogo API client library for .NET.

## Usage

```c#
// All methods require authentication. To get your viagogo OAuth credentials,
// See TODO: docs url
var viagogo = new ViagogoClient(CLIENT_ID,
                                CLIENT_SECRET,
                                new ProductHeaderValue("AwesomeApp", "1.0"));

// Get a list of results that match your search query
var searchResults = await viagogo.Search.GetSearchResultsAsync("FC Barcelona tickets");
```

## Supported Platforms

* .NET 4.5 (Desktop / Server)
* Windows 8 / 8.1 Store Apps
* Windows Phone 8 / 8.1
* Xamarin.iOS / Xamarin.Android / Xamarin.Mac
* Mono 3.x

## Getting Started

HalKit is available on NuGet.

```
Install-Package HalKit
```
