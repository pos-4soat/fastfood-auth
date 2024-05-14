using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Domain.Contracts.Authentication;
using fastfood_auth.Domain.Entity;
using MediatR;

namespace fastfood_auth.Application.UseCases.GuestAuth;

public class GuestAuthHandler(IUserAuthentication userAuthentication) : IRequestHandler<GuestAuthRequest, Result<GuestAuthResponse>>
{
    public async Task<Result<GuestAuthResponse>> Handle(GuestAuthRequest request, CancellationToken cancellationToken)
    {
        UserEntity user = new UserEntity()
        {
            Email = Environment.GetEnvironmentVariable("GUEST_EMAIL"),
            Identification = Environment.GetEnvironmentVariable("GUEST_IDENTIFICATION")
        };

        GuestAuthResponse response = new GuestAuthResponse
        {
            Token = await userAuthentication.AuthenticateUser(user, cancellationToken)
        };

        return Result<GuestAuthResponse>.Success(response);
    }
}
