using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src
{
    internal class HttpResponse : HttpMessage
    {
        public string? HttpVersion { get; private set; }
        public HttpStatusCode? StatusCode { get; private set; }
        public string? ReasonPhrase { get; private set; }
        public HttpResponse(string RawMessageString) 
            : base(RawMessageString)
        {
        }
        public HttpResponse(string HttpVersion, HttpStatusCode StatusCode, Dictionary<string, string>? Headers = null, string? Body = null) 
        {
            this.HttpVersion = HttpVersion;
            this.StatusCode = StatusCode;
            this.ReasonPhrase = StatusCode.GetDisplayName();
            this.StartLine = $"{HttpVersion} {(int)StatusCode} {ReasonPhrase}";
            this.Headers = Headers ?? new Dictionary<string, string>();
            this.Body = Body;
        }

        //RFC 2616:      Status-Line = HTTP-Version SP Status-Code SP Reason-Phrase CRLF
        protected override void ProcessStartLine(string StatusLine)
        {
            if (string.IsNullOrWhiteSpace(StatusLine))
            {
                throw new ArgumentException($"'{nameof(StatusLine)}' cannot be null or whitespace.",
                    nameof(StatusLine));
            }

            string[] SplittedStatusLine = StatusLine.Split(' ');
            HttpVersion = SplittedStatusLine[0] ?? throw new ArgumentNullException($"{HttpVersion} cannot be null!");
            StatusCode = Enum.TryParse(SplittedStatusLine[1], out HttpStatusCode Parsed) 
                ? Parsed : throw new ArgumentNullException($"{StatusCode} cannot be null!");
            ReasonPhrase = SplittedStatusLine[2] ?? throw new ArgumentNullException($"{ReasonPhrase} cannot be null!");
        }

        public static HttpResponse NotImplemented(string HttpVersion)
        {
            return new HttpResponse(HttpVersion, HttpStatusCode.NotImplemented);
        }
        public static HttpResponse NotFound(string HttpVersion)
        {
            return new HttpResponse(HttpVersion, HttpStatusCode.NotFound);
        }
        public static HttpResponse Ok(string HttpVersion, Dictionary<string,string>? Headers = null, string? Body = null)
        {
            return new HttpResponse(HttpVersion, HttpStatusCode.OK, Headers, Body);
        }
    }
}
