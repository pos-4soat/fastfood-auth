using AutoMapper;
using fastfood_auth.Domain.Entity;

namespace fastfood_auth.Application.UseCases.GetUser;

public class GetUsersMapper : Profile
{
    public GetUsersMapper()
    {
        CreateMap<UserEntity, User>();
    }
}
