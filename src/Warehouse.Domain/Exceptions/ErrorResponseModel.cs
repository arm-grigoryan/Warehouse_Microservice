namespace Warehouse.Domain.Exceptions;

public class ErrorResponseModel
{
    public Guid ErrorId { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
}
