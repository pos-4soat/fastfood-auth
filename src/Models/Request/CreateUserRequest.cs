namespace fastfood_auth.Models.Request;

public class CreateUserRequest()
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Identification { get; set; }
}
