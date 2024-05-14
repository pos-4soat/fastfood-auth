using fastfood_auth.Application.Shared.BaseResponse;
using MediatR;

namespace fastfood_auth.Application.UseCases.GuestAuth;

public sealed record GuestAuthRequest : IRequest<Result<GuestAuthResponse>>
{
}
