using Microsoft.AspNetCore.Identity;

namespace TO_DO___API.Models;

public class Usuario : IdentityUser
{
    public string? ImageProfile { get; set; }

    public string Cpf { get; set; }

    public Usuario() : base() { }
}
