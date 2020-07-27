using System.Net;

namespace Web.Exceptions
{
    public class CommandExceptionFor<T> : BaseException
    {
        public CommandExceptionFor(params string[] messages) : 
            base(System.Net.HttpStatusCode.BadRequest, messages)
        {
        }
        public CommandExceptionFor(HttpStatusCode statusCode, params string[] messages) : 
            base(statusCode, messages)
        {
        }
    }
}