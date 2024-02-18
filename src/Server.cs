using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Init");

TcpListener server = new TcpListener(IPAddress.Any, 4221);
server.Start();
using (Socket socket = server.AcceptSocket())
{
    var RequestBuff = new byte[1024];
    _ = socket.Receive(RequestBuff);
    string RequestString = Encoding.ASCII.GetString(RequestBuff);

    string Path = ExtractHttpPath(RequestString);

    byte[] ResponseBuff = new byte[1024];
    if (Path == "/")
    {
        ResponseBuff = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\n\r\n");
    }
    else
    {
        ResponseBuff = Encoding.ASCII.GetBytes("HTTP/1.1 404 Not Found\r\n\r\n");
    }

    socket.Send(ResponseBuff);
}

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