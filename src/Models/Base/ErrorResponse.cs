using fastfood_auth.Constants;
using Newtonsoft.Json;

namespace fastfood_auth.Models.Base;

public class ErrorResponse<TBody> : BaseResponse where TBody : class
{
    [JsonProperty(Order = 3)]
    public TBody? Error { get; set; }

    public ErrorResponse(TBody body) : base(StatusEnum.ERROR) => Error = body;
}
