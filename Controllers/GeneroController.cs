using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class GeneroController : Controller
{
    private readonly IGeneroRepository _generoRepository;

    public GeneroController(IGeneroRepository generoRepository)
    {
        _generoRepository = generoRepository;
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        string? session = HttpContext.Session.GetString("usuario");
        Usuario? usuario = JsonSerializer.Deserialize<Usuario>(session);

        if(usuario.TipoUsuario == EnumTipoUsuario.Administrador)
        {
            return View();
        } else {
            return RedirectToAction("Home", "Usuario");
        }
    }

    [HttpPost]
    public ActionResult Cadastrar(Genero genero)
    {
        _generoRepository.Cadastrar(genero);
        return RedirectToAction("Cadastro", "Jogo");
    }
}