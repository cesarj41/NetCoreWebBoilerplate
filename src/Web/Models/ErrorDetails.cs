using System;
using System.Collections.Generic;

namespace Web.Models
{
    public class ErrorDetails
    {
        public IEnumerable<string> errors { get; private set; }
        public string instance { get; private set; } = $"urn:NovositRU:error:{Guid.NewGuid().ToString()}";

        public ErrorDetails(string error)
        {
            this.errors = new string[]{ error };
        }

        public ErrorDetails(IEnumerable<string> errors)
        {
            this.errors = errors;
        }

    }
}