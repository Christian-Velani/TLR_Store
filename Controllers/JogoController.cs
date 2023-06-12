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

     [HttpPost]
    public ActionResult Index(string nome)
    {
        string? session = HttpContext.Session.GetString("usuario");
        Usuario? usuario = JsonSerializer.Deserialize<Usuario>(session);

        if(usuario.TipoUsuario == EnumTipoUsuario.Administrador)
        {
            return View(_jogoRepository.SearchJogos(nome));
        } else {
            return RedirectToAction("Home", "Usuario");
        }
    }
    
    [HttpGet]
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

    [HttpGet]
    public ActionResult Update(int idJogo)
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
        
        Jogo jogo = _jogoRepository.GetJogo(idJogo);

        string? session = HttpContext.Session.GetString("usuario");
        Usuario? usuario = JsonSerializer.Deserialize<Usuario>(session);

        if(usuario.TipoUsuario == EnumTipoUsuario.Administrador)
        {
            if(jogo != null)
            {
                return View(jogo);
            }

            return NotFound();            
        } else {
            return RedirectToAction("Home", "Usuario");
        }
    }

    [HttpPost]
    public ActionResult Update(Jogo jogo, List<int> tipos, List<int> generos, List<int> desenvolvedoras, List<int> distribuidoras, int idJogo)
    {
        var arquivoImagem = Request.Form.Files["Imagem"];
        if (arquivoImagem != null && arquivoImagem.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                arquivoImagem.CopyTo(ms);
                jogo.Imagem = ms.ToArray();
            }
        } else {
            Jogo jogoRegistrado = _jogoRepository.GetJogo(idJogo); 
            jogo.Imagem = jogoRegistrado.Imagem;
        }
        _jogoRepository.UpdateJogo(idJogo, jogo, generos, tipos, desenvolvedoras, distribuidoras);
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int jogoId)
    {
        _jogoRepository.DeleteJogo(jogoId);
        return RedirectToAction("Index");
    }
}