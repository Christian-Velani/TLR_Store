using Microsoft.AspNetCore.Mvc;

public class AdministradorController : Controller
{
    private readonly IAdministradorRepository _administradorRepository;
    public AdministradorController(IAdministradorRepository administradorRepository)
    {
        _administradorRepository = administradorRepository;
    }

    [HttpGet]
    [Route("Administrador/Index")]
    public ActionResult Index()
    {
        return View(_administradorRepository.GetAllJogos());
    }
    
    [HttpGet]
    [Route("Administrador/Cadastro")]
    public ActionResult Cadastro()
    {
        return View();
    }
    
    [HttpPost]
    [Route("Administrador/Cadastro")]
    public ActionResult Cadastro(Jogo jogo)
    {
        var arquivoImagem = Request.Form.Files["Imagem"];

        if (arquivoImagem != null && arquivoImagem.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                arquivoImagem.CopyTo(ms);
                jogo.Imagem = ms.ToArray();
            }
        }

        _administradorRepository.CadastrarJogo(jogo);

        return RedirectToAction("Index");
    }

  

}