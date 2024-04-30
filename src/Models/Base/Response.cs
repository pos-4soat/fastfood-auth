using fastfood_auth.Constants;
using Newtonsoft.Json;

namespace fastfood_auth.Models.Base;

public class Response<TBody> : BaseResponse where TBody : class
{
    [JsonProperty(Order = 3)]
    public TBody? Body { get; set; }

    public Response(TBody body, StatusEnum status) : base(status) => Body = body;
}
