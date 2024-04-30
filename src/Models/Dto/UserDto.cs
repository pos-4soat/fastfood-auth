using fastfood_auth.Models.Request;

namespace fastfood_auth.Models.Dto;

public class UserDto
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Identification { get; private set; }
    public string CognitoUserIdentification { get; private set; }

    public UserDto(CreateUserRequest request)
    {
        Name = request.Name;
        Email = request.Email;
        Identification = request.Identification;
    }

    public void UpdateCognitoUserId(string cognitoUserId)
    {
        CognitoUserIdentification = cognitoUserId;
    }
}
