using Ecom_Api.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom_Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _Next;
    private readonly IHostEnvironment _Environment;
    private readonly IMemoryCache _MemoryCache;
    private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
    {
        _Next = next;
        _Environment = environment;
        _MemoryCache = memoryCache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _Next(context);
        }
        catch (Exception ex)
        {

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = _Environment.IsDevelopment() ?
                new ApiExceptions((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                : new ApiExceptions((int)HttpStatusCode.InternalServerError, ex.Message);

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
    private bool IsRequestAllowed(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress.ToString();
        var cachkey = $"Rate:{ip}";
        var dateNow = DateTime.Now;

        var (timesTamp, count) = _MemoryCache.GetOrCreate(cachkey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
            return (timesTamp: dateNow, count: 0);
        });

        if (dateNow - timesTamp < _rateLimitWindow)
        {
            if (count > 8)
            {
                return false;
            }
            _MemoryCache.Set(cachkey, (timesTamp, count += 1), _rateLimitWindow);
        }
        else
        {
            _MemoryCache.Set(cachkey, (timesTamp, count), _rateLimitWindow);
        }
        return true;
    }
}
