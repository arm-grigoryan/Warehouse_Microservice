using System.Net;

namespace Warehouse.Domain.Exceptions;

public class WarehouseException : Exception
{
    public int StatusCode { get; }

    public WarehouseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        StatusCode = (int)statusCode;
    }
}
