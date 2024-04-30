using fastfood_auth.Data.Entity;
using fastfood_auth.Interface;
using fastfood_auth.Models.Base;
using fastfood_auth.Models.Dto;
using fastfood_auth.Models.Request;
using fastfood_auth.Models.Response;

namespace fastfood_auth.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly ICognitoService _cognito;

    public UserService(
        IUserRepository repository,
        ICognitoService cognito)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _cognito = cognito ?? throw new ArgumentNullException(nameof(cognito));
    }

    public async Task<Result<CreateUserResponse>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        UserEntity existingCustomer = await _repository.GetUserByCPFOrEmailAsync(request.Identification, request.Email, cancellationToken);

        if (existingCustomer != null)
            return Result<CreateUserResponse>.Failure("ABE001");

        UserDto user = new(request);

        string cognitoUserIdentification = await _cognito.CreateUserAsync(new(user), cancellationToken);
        user.UpdateCognitoUserId(cognitoUserIdentification);
        _ = await _repository.AddUserAsync(new(user), cancellationToken);

        return Result<CreateUserResponse>.Success(new CreateUserResponse(user));
    }

    public async Task<Result<AuthenticateUserResponse>> AuthenticateAsync(string identification, CancellationToken cancellationToken)
    {
        string cpf = identification.Replace(".", string.Empty).Replace("-", string.Empty);

        UserEntity user = await _repository.GetUserByCPFOrEmailAsync(cpf, string.Empty, cancellationToken);

        if (user == null)
            return Result<AuthenticateUserResponse>.Failure("ABE002");

        string token = await _cognito.AuthenticateUserAsync(user, cancellationToken);
        AuthenticateUserResponse authUser = new(user, token);

        return Result<AuthenticateUserResponse>.Success(authUser);
    }

    public async Task<Result<AuthenticateAsGuestResponse>> AnonymousAuthenticateAsync(CancellationToken cancellationToken)
    {
        string email = Environment.GetEnvironmentVariable("GUEST_EMAIL");
        string identification = Environment.GetEnvironmentVariable("GUEST_IDENTIFICATION");

        UserEntity user = new UserEntity(email, identification);

        string token = await _cognito.AuthenticateUserAsync(user, cancellationToken);

        if (string.IsNullOrEmpty(token))
            return Result<AuthenticateAsGuestResponse>.Failure("ABE003");

        AuthenticateAsGuestResponse response = new AuthenticateAsGuestResponse(token);

        return Result<AuthenticateAsGuestResponse>.Success(response);
    }

    public async Task<Result<GetUsersResponse>> GetUsersAsync(CancellationToken cancellationToken)
    {
        IEnumerable<UserEntity> customers = await _repository.GetUsersAsync(cancellationToken);

        List<User> users = [];
        foreach (UserEntity customer in customers)
        {
            User user = new(customer);
            users.Add(user);
        }

        return Result<GetUsersResponse>.Success(new(users));
    }
}
