using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Init");

TcpListener server = new TcpListener(IPAddress.Any, 4221);
server.Start();
using (Socket socket = server.AcceptSocket())
{
    var ResponseBuff =  Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\n\r\n");
    socket.Send(ResponseBuff);
}




