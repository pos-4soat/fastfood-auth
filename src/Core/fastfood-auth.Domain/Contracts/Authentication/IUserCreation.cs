using fastfood_auth.Domain.Entity;

namespace fastfood_auth.Domain.Contracts.Authentication;

public interface IUserCreation
{
    Task<string> CreateUser(UserEntity user, CancellationToken cancellationToken);
    Task DeleteUser(UserEntity user, CancellationToken cancellationToken);
}
