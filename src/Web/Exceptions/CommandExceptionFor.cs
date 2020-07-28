using System.Net;
using Web.Models;

namespace Web.Exceptions
{
    public class CommandExceptionFor<T> : BaseException
    {
        public CommandExceptionFor(params Payload[] messages) : 
            base(System.Net.HttpStatusCode.BadRequest, messages)
        {
        }
        public CommandExceptionFor(HttpStatusCode statusCode, params Payload[] messages) : 
            base(statusCode, messages)
        {
        }
    }
}