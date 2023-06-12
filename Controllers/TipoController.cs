using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class TipoController : Controller
{
    private readonly ITipoRepository _tipoRepository;
    public TipoController(ITipoRepository tipoRepository)
    {
        _tipoRepository = tipoRepository;
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
    public ActionResult Cadastrar(Tipo tipo)
    {
        _tipoRepository.Cadastrar(tipo);
        return RedirectToAction("Cadastro", "Jogo");
    }
}