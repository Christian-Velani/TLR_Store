using Microsoft.AspNetCore.Mvc;
//teste
public class AdministradorController : Controller
{
    private readonly IAdministradorRepository _administradorRepository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly ITipoRepository _tipoRepository;
    private readonly IGeneroRepository _generoRepository;
    public AdministradorController(IAdministradorRepository administradorRepository,
    IEmpresaRepository empresaRepository,
    ITipoRepository tipoRepository,
    IGeneroRepository generoRepository)
    {
        _administradorRepository = administradorRepository;
        _empresaRepository = empresaRepository;
        _tipoRepository = tipoRepository;
        _generoRepository = generoRepository;
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
        var dict = new Dictionary<string, List<Object>>();
        dict["ListaEmpresas"] = new List<Object>(); 
        dict["ListaTipos"] = new List<Object>(); 
        dict["ListaGeneros"] = new List<Object>(); 
        dict["ListaEmpresas"].Add(_empresaRepository.BuscarLista());
        dict["ListaTipos"].Add(_tipoRepository.BuscarLista());
        dict["ListaGeneros"].Add(_generoRepository.BuscarListaCompleta());

        ViewBag.Data = dict;
        return View();
    }
    
    [HttpPost]
    [Route("Administrador/Cadastro")]
    public ActionResult Cadastro(Jogo jogo, List<Empresa> empresas,List<Tipo> tipos)
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