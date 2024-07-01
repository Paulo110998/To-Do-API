using System.ComponentModel.DataAnnotations;

namespace TO_DO___API.Data.Dto;

public class CreateUsuarioDto
{
    public string? ImageProfile { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Email { get; set; }

    public string? Cpf { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string PasswordConfirmation { get; set; }

}
