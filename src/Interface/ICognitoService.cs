using fastfood_auth.Data.Entity;

namespace fastfood_auth.Interface;

public interface ICognitoService
{
    Task<string> AuthenticateUserAsync(UserEntity user, CancellationToken cancellationToken);
    Task<string> CreateUserAsync(UserEntity user, CancellationToken cancellationToken);
}
