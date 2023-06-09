using Microsoft.AspNetCore.Mvc;
//teste
public class JogoController : Controller
{
    private readonly IJogoRepository _jogoRepository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly ITipoRepository _tipoRepository;
    private readonly IGeneroRepository _generoRepository;
    public JogoController(IJogoRepository jogoRepository,
    IEmpresaRepository empresaRepository,
    ITipoRepository tipoRepository,
    IGeneroRepository generoRepository)
    {
        _jogoRepository = jogoRepository;
        _empresaRepository = empresaRepository;
        _tipoRepository = tipoRepository;
        _generoRepository = generoRepository;
    }

    [HttpGet]
    [Route("Jogo/Index")]
    public ActionResult Index()
    {
        return View(_jogoRepository.GetAllJogos());
    }
    
    [HttpGet]
    [Route("Jogo/Cadastro")]
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
    [Route("Jogo/Cadastro")]
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

        _jogoRepository.CadastrarJogo(jogo);

        return RedirectToAction("Index");
    }

  

}