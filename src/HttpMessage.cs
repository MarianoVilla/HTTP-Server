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

        protected Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
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

        }
        public HttpMessage()
        {
            
        }
        protected abstract void ProcessStartLine(string StartLine);

        public override string ToString() => 
                   $"{StartLine}{CRLF}" +
                   $"{string.Join(CRLF, Headers.Select(kvp => $"{kvp.Key}:{kvp.Value}"))}" +
                   $"{CRLF}{CRLF}" +
                   $"{Body}";
    }
}
