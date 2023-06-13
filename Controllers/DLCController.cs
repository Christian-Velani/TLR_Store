using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class DLCController : Controller
{
    private readonly IDLCRepository _dlcRepository;
    
    public DLCController(IDLCRepository dlcRepository)
    {
        this._dlcRepository = dlcRepository;
    }

    public ActionResult ListaDLCsJogo(int idJogo)
    {
        List<DLC> complementos = _dlcRepository.BuscarListaDLCJogo(idJogo);
        return View(complementos);
    }

    [HttpGet]
    public ActionResult Cadastrar(int idJogo)
    {
        string? session = HttpContext.Session.GetString("usuario");
        Usuario? usuario = JsonSerializer.Deserialize<Usuario>(session);

        if(usuario.TipoUsuario == EnumTipoUsuario.Administrador)
        {       
            return View(idJogo);
        } else {
            return RedirectToAction("Home", "Usuario");
        }
    }

    [HttpPost]
    public ActionResult Cadastrar(DLC complemento, int idJogo)
    {
        var arquivoImagem = Request.Form.Files["Imagem"];
        if (arquivoImagem != null && arquivoImagem.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                arquivoImagem.CopyTo(ms);
                complemento.Imagem = ms.ToArray();
            }
        }
        _dlcRepository.Cadastrar(complemento, idJogo);
        return RedirectToAction("Index", "Jogo");
    }
    
    [HttpGet]
    public ActionResult Atualizar(int idComplemento)
    {
        DLC complemento = _dlcRepository.Buscar(idComplemento);

        string? session = HttpContext.Session.GetString("usuario");
        Usuario? usuario = JsonSerializer.Deserialize<Usuario>(session);

        if(usuario.TipoUsuario == EnumTipoUsuario.Administrador)
        {
            if(complemento != null)
            {
                return View(complemento);
            }

            return NotFound();
        } else {
            return RedirectToAction("Home", "Usuario");
        }
    }

    [HttpPost]
    public ActionResult Atualizar(DLC complemento, int idComplemento)
    {
        var arquivoImagem = Request.Form.Files["Imagem"];
        if (arquivoImagem != null && arquivoImagem.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                arquivoImagem.CopyTo(ms);
                complemento.Imagem = ms.ToArray();
            }
        } else {
            DLC complementoRegistrado = _dlcRepository.Buscar(idComplemento); 
            complemento.Imagem = complementoRegistrado.Imagem;
        }
        _dlcRepository.Atualizar(idComplemento, complemento);
        return RedirectToAction("Index", "Jogo");
    }

    public ActionResult Desativar(int idDLC)
    {
        _dlcRepository.Desativar(idDLC);
        return RedirectToAction("Index", "Jogo");
    }

    public ActionResult Detalhes(int idComplemento)
    {
        DLC complemento = _dlcRepository.Buscar(idComplemento);

        string? session = HttpContext.Session.GetString("usuario");
        Usuario? usuario = JsonSerializer.Deserialize<Usuario>(session);

        if(usuario.TipoUsuario == EnumTipoUsuario.Administrador)
        {
            if(complemento != null)
            {
                return View("/Views/DLC/DetalhesAdmin.cshtml", complemento);
            }

            return NotFound();
        } else {
            
            if(complemento != null)
            {
                return View("/Views/DLC/DetalhesComum.cshtml", complemento);
            }

            return NotFound();
        }

    }
}