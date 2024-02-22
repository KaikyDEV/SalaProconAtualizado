using ProBlock.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using ProBlock.Models;
using Microsoft.EntityFrameworkCore;
using ProBlock.Entities;
using ProBlock.Context;

namespace ProBlock.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> usermanager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly AppDbContext _context;

    public AccountController(UserManager<IdentityUser> usermanager, SignInManager<IdentityUser> signInManager, AppDbContext context)
    {
        this.usermanager = usermanager;
        this.signInManager = signInManager;
        this._context = context;
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Register()
    {

        return View();
    }

	[HttpPost]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
            };

            var result = await usermanager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Criando informações pessoais adicionais
                var infoPessoal = new InfoPessoal
                {
                    IdUsuario = user.Id, // Associando o Id do usuário
                    FullName = model.FullName,
                    CPF = model.CPF
                };

                // Adicionando informações pessoais ao contexto e salvando as alterações
                _context.infoPessoals.Add(infoPessoal);
                await _context.SaveChangesAsync();

                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("index", "home");
            }

            ModelState.AddModelError(string.Empty, "Login Inválido");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

}