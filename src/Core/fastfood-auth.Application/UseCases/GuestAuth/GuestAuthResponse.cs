namespace fastfood_auth.Application.UseCases.GuestAuth;

public sealed record GuestAuthResponse
{
    public string Token { get; set; }
}
