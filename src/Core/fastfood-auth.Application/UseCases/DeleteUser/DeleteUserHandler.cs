using AutoMapper;
using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Application.UseCases.CreateUser;
using fastfood_auth.Domain.Contracts.Authentication;
using fastfood_auth.Domain.Contracts.Repository;
using fastfood_auth.Domain.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Application.UseCases.DeleteUser;

public class DeleteUserHandler(
    IUserRepository userRepository,
    IMapper mapper,
    IUserCreation userCreation) : IRequestHandler<DeleteUserRequest, Result<DeleteUserResponse>>
{
    public async Task<Result<DeleteUserResponse>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        UserEntity user = mapper.Map<UserEntity>(request);
        user.Identification = user.Identification.Replace(".", string.Empty).Replace("-", string.Empty);

        Console.WriteLine(user.Identification);

        UserEntity existingCustomer = await userRepository.GetUserByCPFOrEmailAsync(user.Identification, user.Email, cancellationToken);

        Console.WriteLine(existingCustomer.Name);

        if (existingCustomer == null)
            return Result<DeleteUserResponse>.Failure("ABE009");

        await userRepository.DeleteUserAsync(user.Identification, user.Email, cancellationToken);
        Console.WriteLine("delete");
        await userCreation.DeleteUser(user, cancellationToken);
        Console.WriteLine("delete2");

        return Result<DeleteUserResponse>.Success(new DeleteUserResponse());
    }
}

