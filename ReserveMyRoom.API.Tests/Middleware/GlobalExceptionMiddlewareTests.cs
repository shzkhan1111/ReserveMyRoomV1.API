using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using ReserveMyRoom.API.Middleware;

namespace ReserveMyRoom.API.Tests.Middleware;

public class GlobalExceptionMiddlewareTests
{
    [Theory]
    [InlineData(typeof(ArgumentException), 400)]
    [InlineData(typeof(KeyNotFoundException), 404)]
    [InlineData(typeof(InvalidOperationException), 409)]
    [InlineData(typeof(Exception), 500)]
    public async Task InvokeAsync_MapsExceptionsToProblemDetails(
        Type exceptionType,
        int expectedStatus)
    {
        var exception = (Exception)Activator.CreateInstance(
            exceptionType,
            "Test failure")!;
        var middleware = new GlobalExceptionMiddleware(
            _ => throw exception,
            NullLogger<GlobalExceptionMiddleware>.Instance);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        Assert.Equal(expectedStatus, context.Response.StatusCode);
        context.Response.Body.Position = 0;
        using var body = await JsonDocument.ParseAsync(context.Response.Body);
        Assert.Equal(
            expectedStatus,
            body.RootElement.GetProperty("status").GetInt32());

        var detail = body.RootElement.GetProperty("detail").GetString();
        if (expectedStatus == 500)
        {
            Assert.DoesNotContain("Test failure", detail);
        }
        else
        {
            Assert.Equal("Test failure", detail);
        }
    }
}
