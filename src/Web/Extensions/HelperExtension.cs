using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.Models;

namespace Web.Extensions
{
    public static class HelperExtension
    {
        public static Task AppendStatusCodeAsync(
            this RedirectContext<CookieAuthenticationOptions> context, int statusCode)
        {
            context.HttpContext.Response.StatusCode = statusCode;
            return Task.CompletedTask;

        }

        public static IEnumerable<Payload> Errors(this ModelStateDictionary modelState)
        {
            return modelState
                .Select(p => new Payload(
                    p.Key, 
                    p.Value.Errors.Select(p => p.ErrorMessage)));
        }

        public static string[] Errors(this IdentityResult result)
        {
            return result.Errors
                .Select(error => $"{error.Code}: {error.Description}")
                .ToArray();
        }

        public static Task SendAsync<T>(this HttpResponse response, T result)
        {
            response.ContentType = "application/json";
            return response.WriteAsync(result.ToJsonString());
        }

        public static HttpResponse Status(this HttpResponse response, int statusCode)
        {
            response.StatusCode = statusCode;
            return response;
        }

        public static string ToJsonString(this object obj)
        {
            if (obj == null)
            {
                return "";
            }

            if (obj is string)
            {
                return obj as string;
            }

            return JsonSerializer.Serialize(obj);
        }

        public static T FromJsonString<T>(this string jsonString)
        {
            if (jsonString == null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        public static byte[] ToByteArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        public static T FromByteArray<T>(this byte[] byteArray)
        {
            if (byteArray == null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(byteArray);
        }
         
    }
}