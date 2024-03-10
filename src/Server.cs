using codecrafters_http_server.src;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        ILogger Logger = factory.CreateLogger("Program");

        string? Dir = null; 
        if(args is not null && args.Length > 0 )
        {
            Logger.LogInformation($"Received {args.Length} args: {string.Join(", ", args)}");
            if (args[0] == "--directory") 
            {
                if (args.Length != 2)
                {
                    Logger.LogError($"Received --directory arg, but args length wasn't 2; actual length: {args.Length}");
                }
                else
                {
                    Dir = args[1];
                }
            }
        }

        var Server = new HttpServer(IPAddress.Any, 4221, Logger, Dir);

        Server.Start();
        //Console.ReadLine();
    }
}