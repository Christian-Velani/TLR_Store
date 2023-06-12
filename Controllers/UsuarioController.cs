using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class UsuarioController : Controller
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITipoRepository _tipoRepository;
    private readonly IGeneroRepository _generoRepository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IJogoRepository _jogoRepository;
    private List<Jogo>? jogosAux;
    public UsuarioController(IUsuarioRepository usuarioRepository, ITipoRepository tipoRepository, IGeneroRepository generoRepository, IEmpresaRepository empresaRepository, IJogoRepository jogoRepository)
    {
        _usuarioRepository = usuarioRepository;
        _tipoRepository = tipoRepository;
        _generoRepository = generoRepository;
        _empresaRepository = empresaRepository;
        _jogoRepository = jogoRepository;
    }

    [HttpGet]
    public ActionResult Login ()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Login(string login, string senha)
    {
        var usuario = _usuarioRepository.Login(login,senha);
        var carrinho = new Carrinho();

        if (usuario == null)
        {
            ViewBag.Error = "Usuário/Senha inválidos";
            return View();
        }
        else
        {
            HttpContext.Session.SetString("usuario",JsonSerializer.Serialize(usuario));
            HttpContext.Session.SetString("carrinho",JsonSerializer.Serialize(carrinho));
        }
        
        string? session = HttpContext.Session.GetString("usuario");
        Usuario? usuario2 = JsonSerializer.Deserialize<Usuario>(session);

        if(usuario2.TipoUsuario == EnumTipoUsuario.Administrador)
        {
            return RedirectToAction("Index", "Jogo");
        } else {
            return RedirectToAction("Home","Usuario");
        }
    }

    [HttpPost]
    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    [HttpGet]
    [Route("Usuario/Cadastro")]
    public ActionResult Cadastro()
    {
        return View();
    }

    [HttpPost]
    [Route("Usuario/Cadastro")]
    public ActionResult Cadastro(Usuario user)
    {
        var arquivoImagem = Request.Form.Files["Icone"];

        if (arquivoImagem != null && arquivoImagem.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                arquivoImagem.CopyTo(ms);
                user.Icone = ms.ToArray();
            }
        }

        user.JogosAdquiridos = null;
        user.ComplementosAdquiridos = null;
        user.Status = EnumStatus.Ativo;
        _usuarioRepository.Cadastro(user);

        return RedirectToAction("Login");
    }

    [HttpGet]
    [Route("Usuario/Home")]
    public ActionResult Home (int page = 1)
    {
        List<Jogo> jogos = new List<Jogo>();
        jogos = _jogoRepository.GetAllJogos();
        List<Empresa> empresas = new List<Empresa>();
        empresas = _empresaRepository.BuscarLista();
        List<Tipo> tipos = new List<Tipo>();
        tipos = _tipoRepository.BuscarLista();
        List<Genero> generos = new List<Genero>();
        generos = _generoRepository.BuscarListaCompleta();

        ViewBag.empresas = empresas;
        ViewBag.tipos = tipos;
        ViewBag.generos = generos;
        ViewBag.jogos = jogos;

        return View();
    }

    [HttpPost]
    public ActionResult Pesquisar (string? search)
    {
        jogosAux = _jogoRepository.SearchJogos(search);
    
        return RedirectToAction("Home","Usuario");
    }

    [HttpGet]
    public ActionResult Pesquisar()
    {
        ViewBag.jogosAux = jogosAux;
        return RedirectToAction("Home","Usuario");
    }
    
}

