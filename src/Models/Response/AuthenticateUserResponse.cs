using fastfood_auth.Data.Entity;

namespace fastfood_auth.Models.Response;

public sealed record AuthenticateUserResponse
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Identification { get; set; }
    public string Token { get; set; }

    public AuthenticateUserResponse(UserEntity user, string token)
    {
        Name = user.Name;
        Email = user.Email;
        Identification = user.Identification;
        Token = token;
    }
}