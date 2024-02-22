using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProBlock.Context;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using ProBlock.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Globalization;
using System.Text;

namespace ProBlock.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AdminPesquisas : Controller
{

	private readonly UserManager<IdentityUser> _userManager;
	private readonly SignInManager<IdentityUser> _signInManager;
	private readonly AppDbContext _context;

	public AdminPesquisas(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext context)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_context = context;
	}

    [HttpGet("AdminPesquisas/Index")]
    public IActionResult Index(int? page, string Pesquisa = "")
    {
        int pageNumber = page ?? 1;
        int pageSize = 10; // Defina o tamanho da página conforme necessário

        var numerosQuery = _context.PesquisaNumeros.AsQueryable();

        if (!string.IsNullOrEmpty(Pesquisa))
        {
            numerosQuery = numerosQuery
                .Where(c => c.NumeroTelefone.StartsWith(Pesquisa))
                .OrderBy(c => c.NumeroTelefone);
        }

        var numeros = numerosQuery.ToPagedList(pageNumber, pageSize);

        if (numeros.Count <= 0)
        {
            ViewBag.AnyResult = $"Número não encontrado";
        }

        return View(numeros);
    }

    [HttpGet("/AdminPesquisas/FiltrarPorData")]
    public IActionResult FiltrarPorData(DateTime? dataInicio, DateTime? dataFim, int? page)
    {
        int pageNumber = page ?? 1;
        int pageSize = 10; // Defina o tamanho da página conforme necessário

        var query = _context.PesquisaNumeros.AsQueryable();

        // Aplica os filtros de datas à consulta usando a coluna "Detalhes"
        if (dataInicio != null && dataFim != null)
        {
            query = query.Where(p => p.Detalhes >= dataInicio && p.Detalhes <= dataFim);
        }
        else if (dataInicio != null)
        {
            query = query.Where(p => p.Detalhes >= dataInicio);
        }
        else if (dataFim != null)
        {
            query = query.Where(p => p.Detalhes <= dataFim);
        }

        var resultadosFiltrados = query.ToPagedList(pageNumber, pageSize);

        ViewBag.DataInicio = dataInicio;
        ViewBag.DataFim = dataFim;

        return View("Index", resultadosFiltrados);
    }

    [HttpGet("/AdminPesquisas/ExportFiltradoPorData")]
    public IActionResult ExportFiltradoPorData(DateTime? dataInicio, DateTime? dataFim)
    {
        var query = _context.PesquisaNumeros.AsQueryable();

        // Aplica os filtros de datas à consulta usando a coluna "Detalhes"
        if (dataInicio != null && dataFim != null)
        {
            query = query.Where(p => p.Detalhes >= dataInicio && p.Detalhes <= dataFim);
        }
        else if (dataInicio != null)
        {
            query = query.Where(p => p.Detalhes >= dataInicio);
        }
        else if (dataFim != null)
        {
            query = query.Where(p => p.Detalhes <= dataFim);
        }

        var registros = query.ToList(); 

        StringWriter sw = new StringWriter();

        sw.WriteLine($"Id;Número de telefone pesquisado;Usuário;Data e hora");

        // Adicione os dados ao arquivo de texto
        foreach (var registro in registros)
        {
            sw.WriteLine($"{registro.Id};{registro.NumeroTelefone};{registro.ResultadoPesquisa};{registro.Detalhes}");
        }

        // Retorne o arquivo de texto como um FileResult para download
        byte[] data = System.Text.Encoding.UTF8.GetBytes(sw.ToString());
        MemoryStream ms = new MemoryStream(data);
        return File(ms, "text/plain", "historico_pesquisas_filtrado.txt");
    }


    [HttpGet("/AdminPesquisas/ExportarCSV")]
    public IActionResult ExportarCSV(DateTime? dataInicio, DateTime? dataFim)
    {
        var query = _context.PesquisaNumeros.AsQueryable();

        // Aplica os filtros de datas à consulta usando a coluna "Detalhes"
        if (dataInicio != null && dataFim != null)
        {
            query = query.Where(p => p.Detalhes >= dataInicio && p.Detalhes <= dataFim);
        }
        else if (dataInicio != null)
        {
            query = query.Where(p => p.Detalhes >= dataInicio);
        }
        else if (dataFim != null)
        {
            query = query.Where(p => p.Detalhes <= dataFim);
        }

        var registros = query.ToList();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };

        using (var memoryStream = new MemoryStream())
        using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
        using (var csvWriter = new CsvWriter(streamWriter, config))
        {
            csvWriter.WriteField("Id");
            csvWriter.WriteField("Número de telefone pesquisado");
            csvWriter.WriteField("Usuário");
            csvWriter.WriteField("Data e hora");
            csvWriter.NextRecord();

            foreach (var registro in registros)
            {
                csvWriter.WriteField(registro.Id);
                csvWriter.WriteField(registro.NumeroTelefone);
                csvWriter.WriteField(registro.ResultadoPesquisa);
                csvWriter.WriteField(registro.Detalhes);
                csvWriter.NextRecord();
            }

            streamWriter.Flush();
            memoryStream.Position = 0;

            var fileBytes = memoryStream.ToArray();

            return File(fileBytes, "text/csv", "historico_pesquisas_filtrado.csv");
        }
    }

}
