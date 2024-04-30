using fastfood_auth.Constants;
using System.Net;

namespace fastfood_auth.Models.Base;

public record Result
{
    protected Result() { }

    protected Result(string errorCode, HttpStatusCode statusCode, string? errorMessage)
    {
        ErrorCode = errorCode;
        IsFailure = true;
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }

    public string ErrorCode { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public bool IsFailure { get; init; }
    public string? ErrorMessage { get; set; }
}

public sealed record Result<TValue> : Result
{
    protected Result() { }
    private Result(TValue value, StatusEnum status = StatusEnum.SUCCESS)
    {
        Value = value;
        Status = status;
        StatusCode = status switch
        {
            StatusEnum.CREATED => HttpStatusCode.Created,
            _ => HttpStatusCode.OK,
        };
    }
    protected Result(string errorCode, HttpStatusCode statusCode, string? errorMessage = null)
        : base(errorCode, statusCode, errorMessage) { }

    public TValue Value { get; set; }
    public StatusEnum Status { get; set; }

    public new static Result<TValue> Success(TValue value) => new(value);

    public new static Result<TValue> Success(TValue value, StatusEnum status) => new(value, status);

    public static Result<TValue> Failure(string errorCode, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new(errorCode, statusCode, null);
}