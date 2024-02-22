using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ProBlock.Context;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using ProBlock.Areas.Admin.Models;

namespace ProBlock.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AdminUsersController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly AppDbContext _context;

    public AdminUsersController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
	public IActionResult Index()
	{
		var users = _userManager.Users;
		return View(users);
	}

    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            ViewBag.ErrorMessage = $"Usuário com id = {id} não foi encontrado";
            return View("NotFound");
        }
        else
        {
            // Verifica se há registros relacionados na tabela PesquisaNumeros
            var relatedData = _context.PesquisaNumeros.Where(p => p.UsuarioId == id).ToList();

            if (relatedData.Any())
            {
                // Se houver registros relacionados, exclua-os primeiro
                _context.PesquisaNumeros.RemoveRange(relatedData);
                await _context.SaveChangesAsync();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("Index");
        }
    }

}
