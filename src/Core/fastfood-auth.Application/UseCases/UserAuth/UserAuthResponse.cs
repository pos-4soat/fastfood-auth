namespace fastfood_auth.Application.UseCases.UserAuth;

public sealed record UserAuthResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Identification { get; set; }
    public string Token { get; set; }
}
