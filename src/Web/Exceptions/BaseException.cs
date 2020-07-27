using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Web.Exceptions
{
    public abstract class BaseException : Exception
    {
        public int HttpStatusCode { get; private set; }
        private readonly List<string> _errors = new List<string>(); 
        public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();

        public BaseException(HttpStatusCode statusCode, params string[] messages)
            :base(messages.Aggregate((curr, next) => $"{curr},{Environment.NewLine}{next}"))
        {
            _errors.AddRange(messages);
            HttpStatusCode = (int) statusCode;
        }

    }
}