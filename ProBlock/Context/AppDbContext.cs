using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProBlock.Entities;
using ProBlock.Models;

namespace ProBlock.Context;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    public DbSet<NumeroNaoEncontrado> numeroNaoEncontrados { get; set; }

    public DbSet<InfoPessoal> infoPessoals { get; set; }

	public DbSet<PesquisaNumero> PesquisaNumeros { get; set; }

	public DbSet<Numero> Numeros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Numero>().HasData(
            new Numero
            {
                Telefone = "11991086387",
                DataDeCadastro = "05/09/2023"
			});
    }

	internal Task Commit()
	{
		throw new NotImplementedException();
	}
}
