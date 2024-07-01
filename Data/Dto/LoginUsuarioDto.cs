using System.ComponentModel.DataAnnotations;

namespace TO_DO___API.Data.Dto;

public class LoginUsuarioDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}

