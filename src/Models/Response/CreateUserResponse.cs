using fastfood_auth.Models.Dto;

namespace fastfood_auth.Models.Response;

public sealed record CreateUserResponse
{
    public string Name { get; set; }
    public string Email { get; set; }

    public CreateUserResponse(UserDto user)
    {
        Name = user.Name;
        Email = user.Email;
    }
}
