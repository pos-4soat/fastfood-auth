using fastfood_auth.Application.Shared.BaseResponse;
using fastfood_auth.Application.UseCases.CreateUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Application.UseCases.DeleteUser;

public class DeleteUserRequest(string Name, string Email, string Phone, string Identification) :
    IRequest<Result<DeleteUserResponse>>;