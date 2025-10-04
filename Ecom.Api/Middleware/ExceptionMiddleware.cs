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
            ApplySecurity(context);

            if (!IsRequestAllowed(context))
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";

                    var response = new ApiExceptions(
                        (int)HttpStatusCode.TooManyRequests,
                        "Too many requests. Please try again later"
                    );

                    await context.Response.WriteAsJsonAsync(response);
                }

                return; // ⬅️ مهم جداً عشان مايكملش البايبلاين
            }

            await _Next(context);
        }
        catch (Exception ex)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = _Environment.IsDevelopment()
                    ? new ApiExceptions((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                    : new ApiExceptions((int)HttpStatusCode.InternalServerError, "An error occurred");

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
    private bool IsRequestAllowed(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress.ToString();
        var cacheKey = $"Rate:{ip}";
        var now = DateTime.Now;

        var (timeStamp, count) = _MemoryCache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
            return (timeStamp: now, count: 0);
        });

        if (now - timeStamp < _rateLimitWindow)
        {
            if (count > 8)
            {
                return false;
            }
            count++;
            _MemoryCache.Set(cacheKey, (timeStamp, count), _rateLimitWindow);
        }
        else
        {

            _MemoryCache.Set(cacheKey, (now, 1), _rateLimitWindow);
        }

        return true;
    }

    //private void ApplySecurity(HttpContext context)
    //{
    //    context.Response.Headers["X-Content-Type-Options"]= "nosniff";
    //    context.Response.Headers["X-XSS-Protection"]= "1;mode=block";
    //    context.Response.Headers["X-Frame-Options"]= "DENY";
    //}
    private void ApplySecurity(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();
        var headers = context.Response.Headers;

        headers["X-Content-Type-Options"] = "nosniff";

        headers["X-XSS-Protection"] = "1;mode=block";

        headers["X-Frame-Options"] = "DENY";

        if (path is null || (!path.Contains("swagger") && !path.Contains("scalar")))
        {
            headers["Content-Security-Policy"] =
                "default-src 'self'; script-src 'self'; style-src 'self'; object-src 'none'; frame-ancestors 'none';";
        }

        headers["Referrer-Policy"] = "no-referrer";

        headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
    }

}
