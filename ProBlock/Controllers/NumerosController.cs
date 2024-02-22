using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using ProBlock.Entities;
using Microsoft.AspNetCore.Mvc;
using ProBlock.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using X.PagedList;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProBlock.Controllers;

[Authorize]
public class NumerosController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;


    private readonly AppDbContext _context;

    public NumerosController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    public async Task<IActionResult> Index(string Pesquisa = "")
    {
        ViewBag.Termo = await GenerateCommonMessageAsync();

        List<Numero> numeros;

        if (!string.IsNullOrEmpty(Pesquisa))
        {
            numeros = await _context.Numeros
                .Where(c => c.Telefone.StartsWith(Pesquisa))
                .OrderBy(c => c.Telefone)
                .ToListAsync();

            if (numeros.Any())
            {
                ViewBag.BloqueioMessage = "Número bloqueado pelo Procon. Não acionar!";
            }
            else
            {
                ViewBag.AnyResult = $"Número liberado para acionamento. {Pesquisa}";
            }
        }
        else
        {
            ViewBag.AnyResult = $"Número liberado para acionamento. {Pesquisa}";
            var userId = _userManager.GetUserAsync(User).Result.Id;
            var userUser = _userManager.GetUserAsync(User).Result.UserName;

            var modelCreating = new ModelBuilder();

            modelCreating.Entity<PesquisaNumero>().HasData(
                new PesquisaNumero
                {
                    UsuarioId = userId,
                    ResultadoPesquisa = userUser,
                    NumeroTelefone = Pesquisa,
                    Detalhes = DateTime.Now
                });

            numeros = new List<Numero>();
        }
        ViewBag.Name = await NamePesquisa();

        // Movendo a chamada para salvar os resultados da pesquisa
        await SalvarPesquisaNumeros(numeros);

        return View(numeros);
    }

 

    private async Task<string> NamePesquisa()
    {


        var user = await _userManager.GetUserAsync(User);
        string namevalue = string.Empty;

        if (user != null)
        {
            var usuario = await _context.infoPessoals.FirstOrDefaultAsync(u => u.IdUsuario == user.Id);

            if (usuario != null)
            {
                namevalue = usuario.FullName;
            }
        }

        DateTime dataReal = DateTime.Now;

        return $"Pesquisa realizada por {namevalue} na data {dataReal}";
    }


    private async Task<string> GenerateCommonMessageAsync()
    {

        var user = await _userManager.GetUserAsync(User);

        string cpfvalue = string.Empty;
        string namevalue = string.Empty;

        if (user != null)
        {
            var usuario = await _context.infoPessoals.FirstOrDefaultAsync(u => u.IdUsuario == user.Id);

            if (usuario != null)
            {
                cpfvalue = usuario.CPF;
                namevalue = usuario.FullName;
            }
        }

        return $"Eu, {namevalue}, inscrito(a) no CPF sob o número {cpfvalue}," +
            $" reconheço e aceito os " +
            $"termos e condições estabelecidos neste documento como condição " +
            $"para a utilização da plataforma de consulta de números telefônicos." +
            $"\n Declaro que a consulta do número telefônico tem como finalidade exclusiva " +
            $"obter informações sobre o " +
            $"cadastro de referido número junto à plataforma do Procon - Não Me ligue." +
            $"\r\nComprometo-me a utilizar as informações obtidas por meio da consulta de números " +
            $"telefônicos de maneira " +
            $"ética, legal e responsável." +
            $"\nReconheço que sou integralmente responsável por qualquer tipo de acionamento ou contato " +
            $"estabelecido com o número " +
            $"telefônico consultado." +
            $"\nEventuais consequências decorrentes do acionamento não autorizado ou de número devidamente " +
            $"inscrito para bloqueio junto ao Procon, " +
            $"incluindo, mas não se limitando a, interações desagradáveis, disputas legais, ou reclamações, " +
            $"são de minha responsabilidade " +
            $"exclusiva.";
    }

    public async Task SalvarNull(List<PesquisaNumero> numeros)
    {
        var connectionString = "Data Source=SALA143148\\SQLEXPRESS;Initial Catalog=numerosProcon;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;";
        var insertNumeros = "";
    }

    public async Task SalvarPesquisaNumeros(List<Numero> numeros)
    {
        try
        {
            var userId = _userManager.GetUserAsync(User).Result.Id;
            var userUser = _userManager.GetUserAsync(User).Result.UserName;

            var pesquisaNumeros = new List<PesquisaNumero>();

            foreach (var numero in numeros)
            {
                var pesquisaNumero = new PesquisaNumero
                {
                    UsuarioId = userId,
                    ResultadoPesquisa = userUser,
                    NumeroTelefone = numero.Telefone,
                    Detalhes = DateTime.Now
                };

                if (string.IsNullOrEmpty(pesquisaNumero.ResultadoPesquisa))
                {
                    pesquisaNumero.ResultadoPesquisa = string.Empty;
                }

                pesquisaNumeros.Add(pesquisaNumero);
            }

            _context.PesquisaNumeros.AddRange(pesquisaNumeros);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.Write($"Não salvo! Erro: {ex}");
        }
    }



    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Alunos/Create

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Telefone,DataDeCadastro")] Numero numero)
    {
        if (ModelState.IsValid)
        {
            _context.Add(numero);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(numero);
    }

    // GET: 
    public async Task<IActionResult> Details(string? id)
    {
        if (id == null || _context.Numeros == null)
        {
            return NotFound();
        }

        var numero = await _context.Numeros.FirstOrDefaultAsync(m => m.Telefone == id);

        if (numero == null)
        {
            return NotFound();
        }

        return View(numero);
    }

    [HttpGet]
    [Route("/Account/AccessDenied")]
    public ActionResult AccessDenied()
    {

        return View();
    }

}
