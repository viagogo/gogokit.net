using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

[Obsolete("This functionality will be removed in v3.0.0")]
public static class BatchResponseParser
{
    public static IEnumerable<HttpResponseMessage> Parse(string content)
    {
        var delimiterEndIndex = content.IndexOf("\r\n", StringComparison.Ordinal);
        if (delimiterEndIndex > -1)
        {
            // we get the boundary delimeter to split the content into multiple raw request
            var delimiter = content.Substring(0, content.IndexOf("\r\n", StringComparison.Ordinal));
            var sections = content.Split(new[] {delimiter}, StringSplitOptions.RemoveEmptyEntries)
                .Where(sp => sp != "--\r\n")
                .ToList(); // we remove last empty section

            foreach (var section in sections)
            {
                if (section.StartsWith("\r\nContent-Type: application/http; msgtype=response\r\n\r\n"))
                {
                    var response = new HttpResponseMessage();
                    var rawReq = section.Remove(0, section.IndexOf("\r\n\r\n", StringComparison.Ordinal) + 4);
                    response.StatusCode = (HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), rawReq.Substring(9, 3));

                    var headerAndBody = rawReq.Remove(0, rawReq.IndexOf("\r\n", StringComparison.Ordinal) + 2);

                    SetHeadersAndBody(headerAndBody, response);

                    yield return response;
                }

            }
        }
    }

    private static void SetHeadersAndBody(string headerAndBody, HttpResponseMessage response)
    {
        var rawHeaders = headerAndBody.Substring(0, headerAndBody.IndexOf("\r\n\r\n", StringComparison.Ordinal));
        var headers = rawHeaders.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries).ToList();
        var contentHeaders = new List<KeyValuePair<string, string>>();

        foreach (var header in headers)
        {
            var delimeterIdx = header.IndexOf(":", StringComparison.Ordinal);
            var headerKey = header.Substring(0, delimeterIdx);
            var headerValue = header.Remove(0, delimeterIdx + 1).Trim();

            if (contentHeaderKeys.Contains(headerKey))
            {
                contentHeaders.Add(new KeyValuePair<string, string>(headerKey, headerValue));
            }
            else
            {
                response.Headers.Add(headerKey, headerValue);
            }
        }

        SetBody(headerAndBody, contentHeaders, response);
    }

    private static void SetBody(string headerAndBody, IEnumerable<KeyValuePair<string, string>> contentHeaders,
        HttpResponseMessage response)
    {
        var body = string.Empty;

        var bodySeparatorIdx = headerAndBody.IndexOf("\r\n\r\n", StringComparison.Ordinal);
        if (bodySeparatorIdx >= 0)
        {
            body = headerAndBody.Remove(0, bodySeparatorIdx + 4).Trim('\r', '\n');
        }

        response.Content = new StringContent(body);
        response.Content.Headers.Clear();

        foreach (var header in contentHeaders)
        {
            response.Content.Headers.Add(header.Key, header.Value);
        }
    }

    private static readonly HashSet<string> contentHeaderKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Content-Type",
        "Content-Language",
        "Last-Modified",
        "Expires",
        "Allow",
        "Content-Encoding",
        "Content-Length",
        "Content-Range",
        "Content-Disposition"
    };
}