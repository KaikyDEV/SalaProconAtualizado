using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ProBlock.Entities;

public class InfoPessoal
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Id")]
    public string? IdUsuario { get; set; }
    public IdentityUser? Usuario { get; set; }

    [Required]
    [Display(Name = "Nome Completo")]
    public string? FullName { get; set; }

    [Required]
    [Display(Name = "CPF")]
    public string? CPF { get; set; }
}
