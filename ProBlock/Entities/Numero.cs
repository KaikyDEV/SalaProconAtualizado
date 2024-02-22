using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using ProBlock.Context;

namespace ProBlock.Entities;

public class Numero
{
    [Key]
    [Required, MaxLength(15, ErrorMessage = "Limite de 15 caracteres")]
    public string? Telefone { get; set; }

    [Required, MaxLength(10)]
    public string? DataDeCadastro { get; set; }
}

