using AutoMapper;
using fastfood_auth.Domain.Entity;

namespace fastfood_auth.Application.UseCases.UserAuth;

public class UserAuthMapper : Profile
{
    public UserAuthMapper()
    {
        CreateMap<UserEntity, UserAuthResponse>();
    }
}
