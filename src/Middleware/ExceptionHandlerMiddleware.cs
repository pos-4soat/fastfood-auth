using fastfood_auth.Models.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Mime;

namespace fastfood_auth.Middleware;

public class ExceptionHandlerMiddleware(
    RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception)
        {
            JsonSerializerSettings options = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(
                new ErrorResponse<Error>(new Error("ABI999")), options
            ));
        }
    }
}
