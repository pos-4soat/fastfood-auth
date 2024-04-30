namespace fastfood_auth.Models.Response;

public sealed record AuthenticateAsGuestResponse
{
    public string Token { get; private set; }

    public AuthenticateAsGuestResponse(string token)
    {
        Token = token;
    }
}