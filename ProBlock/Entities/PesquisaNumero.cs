using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity; 

namespace ProBlock.Entities;

public class PesquisaNumero
{
	[Key]
	public int Id { get; set; }

	[ForeignKey("Numero")]
	public string? NumeroTelefone { get; set; }
	public Numero? Numero { get; set; }

	public string? ResultadoPesquisa { get; set; }
	public DateTime? Detalhes { get; set; }

	// Propriedade para representar o usuário logado
	public string? UsuarioId { get; set; }
	public IdentityUser? Usuario { get; set; }

}
