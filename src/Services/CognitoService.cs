using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using fastfood_auth.Data.Entity;
using fastfood_auth.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace fastfood_auth.Services;

public class CognitoService(AmazonCognitoIdentityProviderClient cognito, IMemoryCache cache) : ICognitoService
{
    public async Task<string> AuthenticateUserAsync(UserEntity user, CancellationToken cancellationToken)
    {
        if (cache.TryGetValue(user.Identification, out string cachedToken))
            return cachedToken;

        string? userPoolId = Environment.GetEnvironmentVariable("AWS_USER_POOL_ID");
        string? clientId = Environment.GetEnvironmentVariable("AWS_CLIENT_ID_COGNITO");

        Dictionary<string, string> authParameters = new()
        {
            { "USERNAME", user.Email },
            { "PASSWORD", user.Identification }
        };

        AdminInitiateAuthRequest request = new()
        {
            AuthParameters = authParameters,
            ClientId = clientId,
            AuthFlow = "ADMIN_USER_PASSWORD_AUTH",
            UserPoolId = userPoolId
        };

        AdminInitiateAuthResponse response = await cognito.AdminInitiateAuthAsync(request, cancellationToken);

        _ = cache.Set(user.Identification, response.AuthenticationResult.IdToken, TimeSpan.FromMinutes(30));

        return response.AuthenticationResult.IdToken;
    }

    public async Task<string> CreateUserAsync(UserEntity user, CancellationToken cancellationToken)
    {
        string? userPoolId = Environment.GetEnvironmentVariable("AWS_USER_POOL_ID");

        AdminCreateUserRequest request = new()
        {
            UserPoolId = userPoolId,
            Username = user.Email
        };

        AdminCreateUserResponse response = await cognito.AdminCreateUserAsync(request, cancellationToken);

        AdminSetUserPasswordRequest setPassword = new()
        {
            Password = user.Identification,
            Username = user.Email,
            UserPoolId = userPoolId,
            Permanent = true
        };

        _ = await cognito.AdminSetUserPasswordAsync(setPassword, cancellationToken);

        return response.User.Username;
    }
}
