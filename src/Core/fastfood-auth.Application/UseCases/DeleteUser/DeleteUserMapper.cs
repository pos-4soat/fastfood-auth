using AutoMapper;
using fastfood_auth.Application.UseCases.CreateUser;
using fastfood_auth.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastfood_auth.Application.UseCases.DeleteUser;

public class DeleteUserMapper : Profile
{
    public DeleteUserMapper()
    {
        CreateMap<DeleteUserRequest, UserEntity>();
    }
}

