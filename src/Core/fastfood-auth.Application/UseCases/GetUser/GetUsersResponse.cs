﻿namespace fastfood_auth.Application.UseCases.GetUser;

public sealed record GetUsersResponse
{
    public IEnumerable<User> Users { get; set; }
}

public record User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Identification { get; set; }
}
