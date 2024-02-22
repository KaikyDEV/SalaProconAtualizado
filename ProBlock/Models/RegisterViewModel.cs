using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProBlock.Models;

public class RegisterViewModel
{
	[Required]
	[EmailAddress]
	public string? Email { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public string? Password { get; set; }

	[DataType(DataType.Password)]
	[Display(Name = "Confirme a senha")]
	[Compare("Password", ErrorMessage = "As senhas não coincidem")]
	public string? ConfirmPassword { get; set; }

	[Required]
	[Display(Name = "Nome Completo")]
	public string? FullName { get; set; }

	[Required]
	[Display(Name = "CPF")]
	public string? CPF { get; set; }
}