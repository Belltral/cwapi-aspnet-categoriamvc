using CategoriasMvc.Models;
using CategoriasMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace CategoriasMvc.Controllers;

public class AccountController : Controller
{
    private readonly IAutenticacao _autenticacaoService;

    public AccountController(IAutenticacao autenticacaoService)
    {
        _autenticacaoService = autenticacaoService;
    }

    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(UsuarioViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Login inválido");
            return View(model);
        }

        var result = await _autenticacaoService.AutenticaUsuario(model);

        if (result is null)
        {
            ModelState.AddModelError(string.Empty, "Login inválido");
            return View(model);
        }

        Response.Cookies.Append("X-Access-Token", result.Token, new CookieOptions()
        {
            Secure = true, // Protege os cookies durante o transporte
            HttpOnly = true, // Evita ataques XSS
            SameSite = SameSiteMode.Strict // Evita ataques CSRF
        });

        return Redirect("/");
    }
}
