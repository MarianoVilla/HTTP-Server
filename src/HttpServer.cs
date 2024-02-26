using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src
{
    internal class HttpServer : TcpServer
    {
        private string[] SupportedMethods =
        {
            "GET"
        };
        private Encoding DefaultEncoding = Encoding.ASCII;
        private string ServerHttpVersion = "HTTP/1.1";
        public HttpServer(IPAddress Ip, ushort PortNumber, ILogger Logger) 
            : base(Ip, PortNumber, Logger)
        {
        }

        protected override byte[] ProcessRequest(byte[] Bytes)
        {
            string RequestString = DefaultEncoding.GetString(Bytes);
            Logger.LogInformation($"{nameof(RequestString)}: {RequestString}");

            var ParsedRequest = new HttpRequest(RequestString);

            if (string.IsNullOrWhiteSpace(ParsedRequest.RequestUri))
                throw new ArgumentNullException("Request URI cannot be null or white space");

            if (!SupportedMethods.Contains(ParsedRequest.Method))
            {
                return DefaultEncoding.GetBytes(HttpResponse.NotImplemented(ServerHttpVersion).ToString()); 
            }

            if(ParsedRequest.RequestUri == "/")
            {
                return DefaultEncoding.GetBytes(HttpResponse.Ok(ServerHttpVersion).ToString());
            }

            if (ParsedRequest.RequestUri.ToLowerInvariant().StartsWith("/echo/"))
            {
                var Response = HandleEcho(ParsedRequest.RequestUri);

                var httpResponse = HttpResponse.Ok(ServerHttpVersion,
                    new Dictionary<string, string>
                    {
                        { "Content-Type", "text/plain" },
                        { "Content-Length", Response?.Length.ToString() ?? "0" }
                    }, Response);
                    var ResponseAsString = httpResponse.ToString();
                return DefaultEncoding.GetBytes(ResponseAsString);
            }

            return DefaultEncoding.GetBytes(HttpResponse.NotFound(ServerHttpVersion).ToString());

        }
        // /echo/ahsjajad/echo/akdjja
        string HandleEcho(string RequestUri) 
        {
            var ReceivedEcho = RequestUri["/echo/".Length..];
            return ReceivedEcho;
        }
    }
}
