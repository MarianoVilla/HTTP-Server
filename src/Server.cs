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