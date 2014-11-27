# Viagogo.Sdk - viagogo API Client Library for .NET

Viagogo.Sdk is a lightweight async viagogo API client library for .NET.

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