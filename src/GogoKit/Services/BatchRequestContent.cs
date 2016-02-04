// This class is based on https://github.com/mono/aspnetwebstack/blob/master/src/System.Net.Http.Formatting/HttpMessageContent.cs
// Copied this here since it isn't available in the PCL

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GogoLib
{
    /// <summary>
    /// Derived <see cref="T:System.Net.Http.HttpContent"/> class which can encapsulate an <see cref="P:System.Net.Http.HttpMessageContent.HttpResponseMessage"/> or an <see cref="P:System.Net.Http.HttpMessageContent.HttpRequestMessage"/> as an entity with media type "application/http".
    /// </summary>
    public class BatchRequestContent : HttpContent
    {
        private const string DefaultMediaType = "application/http";
        private const string MsgTypeParameter = "msgtype";
        private const string DefaultRequestMsgType = "request";

        private Lazy<Task<Stream>> _lazyContentStream;
        private HttpRequestMessage _httpRequestMessage;
        private bool _contentConsumed;

        private HttpContent Content => _httpRequestMessage.Content;

        public BatchRequestContent(HttpRequestMessage httpRequest)
        {
            _httpRequestMessage = httpRequest;
            Headers.ContentType = new MediaTypeHeaderValue(DefaultMediaType)
            {
                Parameters =
                {
                    new NameValueHeaderValue(MsgTypeParameter, DefaultRequestMsgType)
                }
            };

            InitializeLazyContentStream();
        }

        private void InitializeLazyContentStream()
        {
            _lazyContentStream = new Lazy<Task<Stream>>(() => Content?.ReadAsStreamAsync());
        }

        private static void SerializeHeaderFields(StringBuilder message, HttpHeaders headers)
        {
            if (headers == null)
            {
                return;
            }

            foreach (var keyValuePair in headers)
            {
                message.Append(keyValuePair.Key == "User-Agent"
                    ? $"{keyValuePair.Key}: {string.Join(" ", keyValuePair.Value)}\r\n"
                    : $"{keyValuePair.Key}: {string.Join(", ", keyValuePair.Value)}\r\n");
            }
        }

        private byte[] SerializeHeader()
        {
            var message = new StringBuilder(2048);
            HttpHeaders headers;
            HttpContent content;

            SerializeRequestLine(message, _httpRequestMessage);
            headers = _httpRequestMessage.Headers;
            content = _httpRequestMessage.Content;

            SerializeHeaderFields(message, headers);
            if (content != null)
            {
                SerializeHeaderFields(message, content.Headers);
            }

            message.Append("\r\n");
            return Encoding.UTF8.GetBytes(message.ToString());
        }

        private static void SerializeRequestLine(StringBuilder message, HttpRequestMessage httpRequest)
        {
            message.Append(httpRequest.Method.Method + (object)" ");
            message.Append(httpRequest.RequestUri.PathAndQuery + " ");
            message.Append("HTTP/" + (httpRequest.Version != null ? httpRequest.Version.ToString(2) : "1.1") + "\r\n");
            if (httpRequest.Headers.Host != null)
            {
                return;
            }

            message.Append("Host: " + httpRequest.RequestUri.Authority + "\r\n");
        }

        private static void SerializeStatusLine(StringBuilder message, HttpResponseMessage httpResponse)
        {
            message.Append("HTTP/" + (httpResponse.Version != null ? httpResponse.Version.ToString(2) : "1.1") + " ");
            message.Append(httpResponse.StatusCode + " ");
            message.Append(httpResponse.ReasonPhrase + "\r\n");
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var header = SerializeHeader();
            await stream.WriteAsync(header, 0, header.Length);
            if (Content != null)
            {
                var readStream = await Content.ReadAsStreamAsync();
                ValidateStreamForReading(readStream);
                await Content.CopyToAsync(stream);
            }
        }

        protected override bool TryComputeLength(out long length)
        {
            var flag = _lazyContentStream.Value != null;
            length = 0L;

            if (flag)
            {
                var result = _lazyContentStream.Value.Status == TaskStatus.RanToCompletion ? _lazyContentStream.Value.Result : null;
                if (result == null || !result.CanSeek)
                {
                    length = -1L;
                    return false;
                }

                length = result.Length;
            }

            var numArray = SerializeHeader();
            length += numArray.Length;
            return true;
        }

        private void ValidateStreamForReading(Stream stream)
        {
            if (_contentConsumed)
            {
                if (stream != null && stream.CanRead)
                {
                    stream.Position = 0L;
                }
                else
                {
                    throw new InvalidOperationException("Http message content already read");
                }
            }

            _contentConsumed = true;
        }

        internal static bool ValidateContent(HttpContent content, bool isRequest, out string errorMessge)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var contentType = content.Headers.ContentType;
            if (contentType == null)
            {
                errorMessge = "http message content has invalid media type";
                return false;
            }

            if (!contentType.MediaType.Equals("application/http", StringComparison.OrdinalIgnoreCase))
            {
                errorMessge = "http message content has invalid media type";
                return false;
            }

            var contentTypeParam = contentType
                .Parameters
                .FirstOrDefault(p => p.Name.Equals("msgtype", StringComparison.OrdinalIgnoreCase));

            if (contentTypeParam?.Value == null ||
                !UnquoteToken(contentTypeParam.Value).Equals(isRequest ? "request" : "response", StringComparison.OrdinalIgnoreCase))
            {
                errorMessge = "http message content has invalid media type";
                return false;
            }

            errorMessge = null;
            return true;
        }

        private static string UnquoteToken(string token)
        {
            return string.IsNullOrWhiteSpace(token) || !token.StartsWith("\"", StringComparison.Ordinal) || (!token.EndsWith("\"", StringComparison.Ordinal) || token.Length <= 1)
                ? token
                : token.Substring(1, token.Length - 2);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_httpRequestMessage != null)
                {
                    _httpRequestMessage.Dispose();
                    _httpRequestMessage = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
