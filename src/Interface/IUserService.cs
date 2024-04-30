using fastfood_auth.Models.Base;
using fastfood_auth.Models.Request;
using fastfood_auth.Models.Response;

namespace fastfood_auth.Interface;

public interface IUserService
{
    Task<Result<CreateUserResponse>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken);
    Task<Result<AuthenticateUserResponse>> AuthenticateAsync(string identification, CancellationToken cancellationToken);
    Task<Result<AuthenticateAsGuestResponse>> AnonymousAuthenticateAsync(CancellationToken cancellationToken);
    Task<Result<GetUsersResponse>> GetUsersAsync(CancellationToken cancellationToken);
}
