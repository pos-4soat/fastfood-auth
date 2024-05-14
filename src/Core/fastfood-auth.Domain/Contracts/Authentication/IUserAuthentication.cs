using fastfood_auth.Domain.Entity;

namespace fastfood_auth.Domain.Contracts.Authentication;

public interface IUserAuthentication
{
    Task<string> AuthenticateUser(UserEntity user, CancellationToken cancellationToken);
}
