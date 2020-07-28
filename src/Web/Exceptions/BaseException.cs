using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Web.Models;

namespace Web.Exceptions
{
    public abstract class BaseException : Exception
    {
        public int HttpStatusCode { get; private set; }
        private readonly List<Payload> _errors = new List<Payload>(); 
        public IReadOnlyCollection<Payload> Errors => _errors.AsReadOnly();
        
        public BaseException(HttpStatusCode statusCode, params Payload[] messages)
            :base(String.Join(",", messages.Select(msg => String.Join(",", msg.Error.Value))))
        {
            foreach (var item in messages)
            {
                _errors.Add(item);
            }
            HttpStatusCode = (int) statusCode;
        }

    }
}