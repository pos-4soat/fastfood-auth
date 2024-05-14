using AutoMapper;
using fastfood_auth.Domain.Entity;

namespace fastfood_auth.Application.UseCases.CreateUser;

public class CreateUserMapper : Profile
{
    public CreateUserMapper()
    {
        CreateMap<CreateUserRequest, UserEntity>();
    }
}
