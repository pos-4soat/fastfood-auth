using fastfood_auth.Models.Dto;
using System.Text.Json.Serialization;

namespace fastfood_auth.Data.Entity;

public class UserEntity
{
    [JsonPropertyName("pk")]
    public string Pk => Identification;

    [JsonPropertyName("sk")]
    public string Sk => Pk;

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("identification")]
    public string Identification { get; set; }

    [JsonPropertyName("cognitoUserIdentification")]
    public string CognitoUserIdentification { get; set; }

    public UserEntity(UserDto user)
    {
        Name = user.Name;
        Email = user.Email;
        Identification = user.Identification;
        CognitoUserIdentification = user.CognitoUserIdentification;
    }

    public UserEntity(string email, string identification)
    {
        Email = email;
        Identification = identification;
    }

    public UserEntity(string? identification, string? name, string? email, string? clientId)
    {
        Name = name;
        Email = email;
        Identification = identification;
        CognitoUserIdentification = clientId;
    }
}