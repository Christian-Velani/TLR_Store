using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class EmpresaController : Controller
{
    private readonly IEmpresaRepository _empresaRepository;

    public EmpresaController(IEmpresaRepository empresaRepository)
    {
        _empresaRepository = empresaRepository;
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
            return RedirectToAction("Cadastro", "Jogo");
        }
    }

    [HttpPost]
    public ActionResult Cadastrar(Empresa empresa)
    {
        _empresaRepository.Cadastrar(empresa);
        return RedirectToAction("Cadastro", "Jogo");
    }
}