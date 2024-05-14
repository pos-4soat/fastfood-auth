using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using fastfood_auth.Domain.Contracts.Authentication;
using fastfood_auth.Domain.Entity;

namespace fastfood_auth.Infra.Cognito.Creation;

public class CognitoUserCreation(AmazonCognitoIdentityProviderClient cognito) : IUserCreation
{
    public async Task<string> CreateUser(UserEntity user, CancellationToken cancellationToken)
    {
        string? userPoolId = Environment.GetEnvironmentVariable("AWS_USER_POOL_ID");

        AdminCreateUserRequest request = new AdminCreateUserRequest()
        {
            UserPoolId = userPoolId,
            Username = user.Email
        };

        AdminCreateUserResponse response = await cognito.AdminCreateUserAsync(request, cancellationToken);

        AdminSetUserPasswordRequest setPassword = new AdminSetUserPasswordRequest()
        {
            Password = user.Identification,
            Username = user.Email,
            UserPoolId = userPoolId,
            Permanent = true
        };

        await cognito.AdminSetUserPasswordAsync(setPassword, cancellationToken);

        return response.User.Username;
    }
}
