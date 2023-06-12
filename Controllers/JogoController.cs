using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
    public ActionResult Index()
    {
        string? session = HttpContext.Session.GetString("usuario");
        Usuario? usuario = JsonSerializer.Deserialize<Usuario>(session);

        if(usuario.TipoUsuario == EnumTipoUsuario.Administrador)
        {
            return View(_jogoRepository.GetAllJogos());
        } else {
            return RedirectToAction("Home", "Usuario");
        }
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
    public ActionResult Cadastro(Jogo jogo, List<int> tipos, List<int> generos, List<int> desenvolvedoras, List<int> distribuidoras)
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

        _jogoRepository.CadastrarJogo(jogo, generos, tipos, desenvolvedoras, distribuidoras);

        return RedirectToAction("Index");
    }

}