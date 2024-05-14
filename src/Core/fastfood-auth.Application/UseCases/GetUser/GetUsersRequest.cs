using fastfood_auth.Application.Shared.BaseResponse;
using MediatR;

namespace fastfood_auth.Application.UseCases.GetUser;

public sealed record GetUsersRequest : IRequest<Result<GetUsersResponse>>
{

}
