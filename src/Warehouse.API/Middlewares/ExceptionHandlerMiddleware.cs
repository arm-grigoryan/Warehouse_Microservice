using FluentValidation;
using System.Net;
using System.Text.Json;
using Warehouse.Domain.Exceptions;

namespace Warehouse.API.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponseModel
        {
            ErrorId = Guid.NewGuid(),
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message = "An unexpected error occurred."
        };

        switch (exception)
        {
            case WarehouseException warehouseException:
                response.StatusCode = warehouseException.StatusCode;
                response.Message = warehouseException.Message;
                break;

            default:
                response.Message = exception.Message;
                break;
        }

        context.Response.StatusCode = response.StatusCode;

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
