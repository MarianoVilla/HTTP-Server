using codecrafters_http_server.src;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger Logger = factory.CreateLogger("Program");
var Server = new HttpServer(IPAddress.Any, 4221, Logger);

Server.Start();

//TcpListener Server = new TcpListener(IPAddress.Any, 4221);
//Server.Start();
//using (Socket socket = Server.AcceptSocket())
//{
//    var RequestBuff = new byte[1024];
//    _ = socket.Receive(RequestBuff);
//    string RequestString = Encoding.ASCII.GetString(RequestBuff);

//    string Path = ExtractHttpPath(RequestString);
//    byte[] ResponseBuff = new byte[1024];

//    if (Path == "/")
//    {
//        ResponseBuff = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\n\r\n");
//        socket.Send(ResponseBuff);
//        return;
//    }
//    var LoweredPath = Path.ToLower();
//    string[] SplittedPath = LoweredPath.Split('/');
//    if (SplittedPath[1] == "echo")
//    {
//        // /echo/falopa/echo/falopa
//        string ReceivedEcho = LoweredPath.Split("/echo/")[1];
//        ResponseBuff = Encoding.ASCII.GetBytes($"HTTP/1.1 200 OK" +
//            $"\r\nContent-Type: text/plain" +
//            $"\r\nContent-Length: {ReceivedEcho.Length}" +
//            $"\r\n\r\n{ReceivedEcho}");
//        socket.Send(ResponseBuff);
//        return;
//    }
//    ResponseBuff = Encoding.ASCII.GetBytes("HTTP/1.1 404 Not Found\r\n\r\n");
//    socket.Send(ResponseBuff);
//}

string ExtractHttpPath(string? RequestString)
{
    if (string.IsNullOrWhiteSpace(RequestString))
    {
        throw new ArgumentException($"'{nameof(RequestString)}' cannot be null or whitespace.", nameof(RequestString));
    }

    var RequestLines = RequestString.Split('\n');
    Debug.Assert(RequestLines.Length > 0, "HTTP request should have at least 1 line!");

    var SplittedFirstLine = RequestLines[0].Split(' ');
    Debug.Assert(SplittedFirstLine.Length == 3, "Start line should have three space-separated values!");

    var Path = SplittedFirstLine[1];
    Debug.Assert(!string.IsNullOrWhiteSpace(Path), "The path shouldn't be empty!");

    return Path;
}