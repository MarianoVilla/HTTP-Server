using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src
{
    internal abstract class HttpMessage
    {
        protected string CRLF = "\r\n";
        public string RawMessageString { get; }
        protected string StartLine { get; set; }
        public string? Body { get; set; }

        public Dictionary<string, string> Headers { get; protected set; } = new Dictionary<string, string>();
        public HttpMessage(string RawMessageString)
        {
            if (string.IsNullOrWhiteSpace(RawMessageString))
            {
                throw new ArgumentException($"'{nameof(RawMessageString)}' cannot be null or whitespace.",
                    nameof(RawMessageString));
            }

            this.RawMessageString = RawMessageString;

            var MessageLines = RawMessageString.Split(CRLF);
            Debug.Assert(MessageLines.Length > 0, "HTTP message should have at least a Start Line!");

            StartLine = MessageLines[0];
            ProcessStartLine(StartLine);
            ProcessHeaders(MessageLines.Skip(1));
        }
        public HttpMessage()
        {
            
        }
        protected abstract void ProcessStartLine(string StartLine);
        protected void ProcessHeaders(IEnumerable<string> RawHeaders)
        {
            foreach (var RH in RawHeaders)
            {
                if (RH == "")
                    break;
                if (RH.Contains(":"))
                {
                    var SplittedLine = RH.Split(':');
                    Headers.Add(SplittedLine[0], string.Join(':', SplittedLine[1..]));
                }
            }
        }

        public override string ToString() => 
                   $"{StartLine}{CRLF}" +
                   $"{string.Join(CRLF, Headers.Select(kvp => $"{kvp.Key}:{kvp.Value}"))}" +
                   $"{CRLF}{CRLF}" +
                   $"{Body}";
    }
}
