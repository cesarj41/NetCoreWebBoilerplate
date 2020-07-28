using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Models
{
    public class ErrorDetails
    {
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> errors { get; private set; }
        public string instance { get; private set; } = $"urn:AppName:error:{Guid.NewGuid().ToString()}";

        public ErrorDetails(params Payload[] payloads)
        {
            this.errors =  payloads.Select(p => p.Error);
        }

        public ErrorDetails(IEnumerable<Payload> payloads)
        {
            this.errors =  payloads.Select(p => p.Error);
        }

    }
    public class Payload
    {
        public KeyValuePair<string, IEnumerable<string>> Error;

        public Payload(string key = "message", params string[] values)
        {
            Error = new KeyValuePair<string, IEnumerable<string>>(key, values);
        }

        public Payload(string key,  IEnumerable<string> values)
        {
            Error = new KeyValuePair<string, IEnumerable<string>>(key, values);
        }
    }
}