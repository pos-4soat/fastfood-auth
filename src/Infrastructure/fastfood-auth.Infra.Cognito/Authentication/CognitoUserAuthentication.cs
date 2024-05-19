using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using fastfood_auth.Domain.Contracts.Authentication;
using fastfood_auth.Domain.Entity;
using Microsoft.Extensions.Caching.Memory;

namespace fastfood_auth.Infra.Cognito.Authentication;

public class CognitoUserAuthentication(AmazonCognitoIdentityProviderClient cognito, IMemoryCache cache) : IUserAuthentication
{
    public async Task<string> AuthenticateUser(UserEntity user, CancellationToken cancellationToken)
    {
        if (cache.TryGetValue(user.Identification, out string cachedToken))
            return cachedToken;

        string? userPoolId = Environment.GetEnvironmentVariable("AWS_USER_POOL_ID");
        string? clientId = Environment.GetEnvironmentVariable("AWS_CLIENT_ID_COGNITO");

        Dictionary<string, string> authParameters = new Dictionary<string, string>
    {
        { "USERNAME", user.Email },
        { "PASSWORD", user.Identification }
    };

        AdminInitiateAuthRequest request = new AdminInitiateAuthRequest()
        {
            AuthParameters = authParameters,
            ClientId = clientId,
            AuthFlow = "ADMIN_USER_PASSWORD_AUTH",
            UserPoolId = userPoolId
        };

        AdminInitiateAuthResponse response = await cognito.AdminInitiateAuthAsync(request, cancellationToken);

        try
        {
            cache.Set(user.Identification, response.AuthenticationResult.IdToken, TimeSpan.FromMinutes(30));
        }
        catch{}

        return response.AuthenticationResult.IdToken;
    }
}
