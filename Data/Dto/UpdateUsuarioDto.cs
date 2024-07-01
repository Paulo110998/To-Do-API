using System.ComponentModel.DataAnnotations;

namespace TO_DO___API.Data.Dto;

public class UpdateUsuarioDto
{
    public string? ImageProfile { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Email { get; set; }

    public string? Cpf { get; set; }

}
