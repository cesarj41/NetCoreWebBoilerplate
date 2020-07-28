using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Models
{
    public class ErrorDetails
    {
        public string traceId { get; private set; } = $"AppName:{Guid.NewGuid().ToString()}";
        public Dictionary<string, IEnumerable<string>> errors { get; private set; }
        public ErrorDetails(params Payload[] payloads)
        {
            this.errors = new Dictionary<string, IEnumerable<string>>(
                payloads.Select(p => p.Error)
            );
        }

        public ErrorDetails(IEnumerable<Payload> payloads)
        {
            this.errors = new Dictionary<string, IEnumerable<string>>(
                payloads.Select(p => p.Error)
            );
        }

    }
    public class Payload
    {
        public KeyValuePair<string, IEnumerable<string>> Error;

        public Payload(string key = "messages", params string[] values)
        {
            Error = new KeyValuePair<string, IEnumerable<string>>(key, values);
        }

        public Payload(string key,  IEnumerable<string> values)
        {
            Error = new KeyValuePair<string, IEnumerable<string>>(key, values);
        }
    }
}