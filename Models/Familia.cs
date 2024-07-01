using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TO_DO___API.Models;

public class Familia
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string TituloRedeFamilia { get; set; }

    [Required]
    public string CreatedByUserId { get; set; }

    [ForeignKey("CreatedByUserId")]
    public virtual Usuario CreatedByUser { get; set; }

    public virtual ICollection<MembrosFamilia> membrosFamilias { get; set; }

}

