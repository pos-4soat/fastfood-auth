using fastfood_auth.Data.Entity;

namespace fastfood_auth.Interface;

public interface IUserRepository
{
    Task<bool> AddUserAsync(UserEntity customer, CancellationToken cancellationToken);
    Task<UserEntity> GetUserByCPFOrEmailAsync(string identification, string email, CancellationToken cancellationToken);
    Task<IEnumerable<UserEntity>> GetUsersAsync(CancellationToken cancellationToken);
}
