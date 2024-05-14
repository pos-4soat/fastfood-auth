using AutoMapper;
using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Domain.Contracts.Repository;
using MediatR;

namespace fastfood_auth.Application.UseCases.GetUser;

public class GetUsersHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetUsersRequest, Result<GetUsersResponse>>
{
    public async Task<Result<GetUsersResponse>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entity.UserEntity> customers = await userRepository.GetUsersAsync(cancellationToken);

        IEnumerable<User> customersDto = mapper.Map<IEnumerable<User>>(customers);

        return Result<GetUsersResponse>.Success(new GetUsersResponse() { Users = customersDto });
    }
}
