using fastfood_auth.Domain.Enum;
using Newtonsoft.Json;

namespace fastfood_auth.Application.Shared.BaseResponse;

public class Response<TBody> : BaseResponse where TBody : class
{
    [JsonProperty(Order = 3)]
    public TBody? Body { get; set; }

    public Response(TBody body, StatusResponse status) : base(status) => Body = body;
}