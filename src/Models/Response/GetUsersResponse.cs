using fastfood_auth.Data.Entity;

namespace fastfood_auth.Models.Response;

public sealed record GetUsersResponse
{
    public IEnumerable<User> Users { get; set; }

    public GetUsersResponse(IEnumerable<User> users)
    {
        Users = users;
    }
}

public record User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Identification { get; set; }

    public User(UserEntity customer)
    {
        Name = customer.Name;
        Email = customer.Email;
        Identification = customer.Identification;
    }
}