using fastfood_auth.Domain.Entity;

namespace fastfood_auth.Domain.Contracts.Repository;

public interface IUserRepository
{
    Task<bool> AddUserAsync(UserEntity customer, CancellationToken cancellationToken);
    Task<UserEntity> GetUserByCPFOrEmailAsync(string identification, string email, CancellationToken cancellationToken);
    Task<IEnumerable<UserEntity>> GetUsersAsync(CancellationToken cancellationToken);
}
