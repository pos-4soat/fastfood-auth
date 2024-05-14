using AutoMapper;
using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Domain.Contracts.Authentication;
using fastfood_auth.Domain.Contracts.Repository;
using fastfood_auth.Domain.Entity;
using MediatR;

namespace fastfood_auth.Application.UseCases.CreateUser;

public class CreateUserHandler(
    IUserRepository userRepository,
    IMapper mapper,
    IUserCreation userCreation) : IRequestHandler<CreateUserRequest, Result<CreateUserResponse>>
{
    public async Task<Result<CreateUserResponse>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        UserEntity user = mapper.Map<UserEntity>(request);
        user.Identification = user.Identification.Replace(".", string.Empty).Replace("-", string.Empty);

        UserEntity existingCustomer = await userRepository.GetUserByCPFOrEmailAsync(user.Identification, user.Email, cancellationToken);

        if (existingCustomer != null)
            return Result<CreateUserResponse>.Failure("ABE008");

        string cognitoUserIdentification = await userCreation.CreateUser(user, cancellationToken);
        user.CognitoUserIdentification = cognitoUserIdentification;
        await userRepository.AddUserAsync(user, cancellationToken);

        return Result<CreateUserResponse>.Success(new CreateUserResponse());
    }
}
