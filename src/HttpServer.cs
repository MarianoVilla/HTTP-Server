using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization.Metadata;
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

        protected override async Task ProcessRequestAsync(Socket socket)
        {
            try
            {
                var Bytes = new byte[MaxRecvBytes];
                int ReceivedBytesCount = await socket.ReceiveAsync(Bytes, SocketFlags.None);
                Logger.LogInformation($"{nameof(ReceivedBytesCount)}: {ReceivedBytesCount}");

                Logger.LogInformation($"------- Thread {Thread.CurrentThread.Name} {Thread.CurrentThread.ManagedThreadId} processing request");
                string RequestString = DefaultEncoding.GetString(Bytes);
                Logger.LogInformation($"{nameof(RequestString)}: {RequestString}");

                var ParsedRequest = new HttpRequest(RequestString);

                if (string.IsNullOrWhiteSpace(ParsedRequest.RequestUri))
                    throw new ArgumentNullException("Request URI cannot be null or white space");

                if (!SupportedMethods.Contains(ParsedRequest.Method))
                {
                    await socket.SendAsync(DefaultEncoding.GetBytes(HttpResponse.NotImplemented(ServerHttpVersion).ToString()), SocketFlags.None);
                    return;
                }

                switch (ParsedRequest.RequestUri.ToUpperInvariant())
                {
                    case Routes.Base: 
                        await socket.SendAsync(DefaultEncoding.GetBytes(HttpResponse.Ok(ServerHttpVersion).ToString()), SocketFlags.None); 
                        break;

                    case var uri when uri.StartsWith(Routes.Echo):
                        await Echo(ParsedRequest);
                        break;

                    case var uri when uri.StartsWith(Routes.UserAgent):
                        await UserAgent(ParsedRequest);
                        break;

                    default: 
                        await socket.SendAsync(DefaultEncoding.GetBytes(HttpResponse.NotFound(ServerHttpVersion).ToString()), SocketFlags.None); 
                        break; 
                }

                //if (ParsedRequest.RequestUri == Routes.Base)
                //{
                //    await socket.SendAsync(DefaultEncoding.GetBytes(HttpResponse.Ok(ServerHttpVersion).ToString()), SocketFlags.None);
                //    return;
                //}

                //if (ParsedRequest.RequestUri.ToLowerInvariant().StartsWith(Routes.Echo))
                //{
                //    await Echo(ParsedRequest);
                //    return;
                //}

                //if (ParsedRequest.RequestUri.ToLowerInvariant().StartsWith(Routes.UserAgent))
                //{
                //    await UserAgent(ParsedRequest);
                //    return;
                //}

                //await socket.SendAsync(DefaultEncoding.GetBytes(HttpResponse.NotFound(ServerHttpVersion).ToString()), SocketFlags.None);
                //return;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                socket.Close();
            }

            async Task Echo(HttpRequest ParsedRequest)
            {
                var Response = ParsedRequest.RequestUri?[$"{Routes.Echo}/".Length..];
                var httpResponse = HttpResponse.Ok(ServerHttpVersion,
                    new Dictionary<string, string>
                    {
                        { HttpHeaderConstants.ContentType, HttpHeaderConstants.TextPlain },
                        { HttpHeaderConstants.ContentLength, Response?.Length.ToString() ?? "0" }
                    }, Response);
                var ResponseAsString = httpResponse.ToString();
                await socket.SendAsync(DefaultEncoding.GetBytes(ResponseAsString), SocketFlags.None);
            }

            async Task UserAgent(HttpRequest ParsedRequest)
            {
                var UserAgent = ParsedRequest.Headers[HttpHeaderConstants.UserAgent]?.Trim();
                var Response = HttpResponse.Ok(ServerHttpVersion, new Dictionary<string, string>()
                    {
                        { HttpHeaderConstants.ContentType, HttpHeaderConstants.TextPlain },
                        { HttpHeaderConstants.ContentLength, UserAgent?.Length.ToString() ?? "0" }
                    }, UserAgent);
                await socket.SendAsync(DefaultEncoding.GetBytes(Response.ToString()), SocketFlags.None);
            }
        }
    }
}
