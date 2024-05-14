using AutoMapper;
using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Domain.Contracts.Authentication;
using fastfood_auth.Domain.Contracts.Repository;
using MediatR;

namespace fastfood_auth.Application.UseCases.UserAuth;

public class UserAuthHandler(IUserRepository userRepository, IMapper mapper, IUserAuthentication userAuthentication) : IRequestHandler<UserAuthRequest, Result<UserAuthResponse>>
{
    public async Task<Result<UserAuthResponse>> Handle(UserAuthRequest request, CancellationToken cancellationToken)
    {
        string cpf = request.cpf.Replace(".", string.Empty).Replace("-", string.Empty);

        Domain.Entity.UserEntity user = await userRepository.GetUserByCPFOrEmailAsync(cpf, string.Empty, cancellationToken);

        if (user == null)
            return Result<UserAuthResponse>.Failure("ABE009");

        UserAuthResponse response = mapper.Map<UserAuthResponse>(user);
        response.Token = await userAuthentication.AuthenticateUser(user, cancellationToken);

        return Result<UserAuthResponse>.Success(response);
    }
}
