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
        List<Genero> generos = new List<Genero>();
        generos = _generoRepository.BuscarListaCompleta();
        List<Tipo> tipos = new List<Tipo>();
        tipos = _tipoRepository.BuscarLista();
        List<Empresa> empresas = new List<Empresa>();
        empresas = _empresaRepository.BuscarLista();

        ViewBag.generos = generos;
        ViewBag.tipos = tipos;
        ViewBag.empresas = empresas;

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