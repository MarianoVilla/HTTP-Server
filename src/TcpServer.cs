using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src
{
    internal abstract class TcpServer
    {
        public IPAddress Ip { get; }
        public ushort PortNumber { get; }
        public ILogger Logger { get; }
        protected int MaxRecvBytes { get; set; }
        private bool ShouldStop = false;
        public TcpServer(IPAddress Ip, ushort PortNumber, ILogger Logger)
        {
            this.Ip = Ip;
            this.PortNumber = PortNumber;
            this.Logger = Logger;
            this.MaxRecvBytes = 1024;
        }


        public void Start()
        {
            TcpListener Server = new TcpListener(Ip, PortNumber);
            Server.Start();
            Logger.LogInformation($"Started server on {Ip}:{PortNumber}");
            //while (!ShouldStop)
            //{
                using (Socket socket = Server.AcceptSocket())
                {
                        var RequestBuff = new byte[MaxRecvBytes];
                        int ReceivedBytesCount = socket.Receive(RequestBuff);
                        Logger.LogInformation($"{nameof(ReceivedBytesCount)}: {ReceivedBytesCount}");
                        byte[] Response = ProcessRequest(RequestBuff);
                        socket.Send(Response);
                }
            //}

        }
        public void Stop()
        {
            ShouldStop = true;
        }
        protected abstract byte[] ProcessRequest(byte[] Bytes);

    }
}
