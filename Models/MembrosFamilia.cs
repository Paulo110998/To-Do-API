using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TO_DO___API.Models;

public enum VinculoFamilia
{
    Filho, // 0
    Pai, // 1
    Mãe, // 3
    Irmão, // 4
    Tio, // 5
    Primo, // 6
    Avô, // 7
    Amigo // 8
}

public class MembrosFamilia
{
    [Key]
    [Required]
    public int Id { get; set; }

    [StringLength(130)] // AQUI É "VARCHAR(130)"
    public string NomeMembro { get; set; }

    [Required]
    public VinculoFamilia VinculoFamilia { get; set; } // AQUI É "LONGTEXT"

    [Required]
    public int? FamiliaId { get; set; }   // AQUI É "INT

    public virtual Familia Familia { get; set; }

    [Required]
    public string CreatedByUserId { get; set; }  // varcahr(110)

    [ForeignKey("CreatedByUserId")]
    public virtual Usuario CreatedByUser { get; set; }

}