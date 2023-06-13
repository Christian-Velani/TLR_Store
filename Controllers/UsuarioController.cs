using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class UsuarioController : Controller
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITipoRepository _tipoRepository;
    private readonly IGeneroRepository _generoRepository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IJogoRepository _jogoRepository;
    private readonly IDLCRepository _dlcRepository;
    public UsuarioController(IUsuarioRepository usuarioRepository, ITipoRepository tipoRepository, IGeneroRepository generoRepository, IEmpresaRepository empresaRepository, IJogoRepository jogoRepository, IDLCRepository dlcRepository)
    {
        _usuarioRepository = usuarioRepository;
        _tipoRepository = tipoRepository;
        _generoRepository = generoRepository;
        _empresaRepository = empresaRepository;
        _jogoRepository = jogoRepository;
        _dlcRepository = dlcRepository;
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

    [HttpGet]
    public ActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    [HttpGet]
    public ActionResult Cadastro()
    {
        return View();
    }

    [HttpPost]
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
    public ActionResult Home ()
    {
        List<Jogo> jogos = new List<Jogo>();
        jogos = _jogoRepository.GetAllJogosAtivos();
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

    public ActionResult Home (string nome, int GenerosId, int TiposId, int EmpresasId)
    {
        List<Jogo> jogos = new List<Jogo>();
        List<int> jogos1 = new List<int>();
        List<int> jogos2 = new List<int>();
        List<int> jogos3 = new List<int>();
        List<int> jogos4 = new List<int>();  
        List<int> jogosfinal = new List<int>();
        List<Jogo> jogosfiltrados = new List<Jogo>();      
        List<Empresa> empresas = new List<Empresa>();        
        List<Tipo> tipos = new List<Tipo>();        
        List<Genero> generos = new List<Genero>();

        jogos = _jogoRepository.GetAllJogosAtivos();

        if(nome != null)
        {
            jogos1 = _jogoRepository.SearchJogosNome(nome);
        }

        if(GenerosId != 0)
        {
            jogos2 = _jogoRepository.JogosGeneros(GenerosId);
        }

        if(TiposId != 0)
        {
            jogos3 = _jogoRepository.JogosTipos(TiposId);
        }

        if(EmpresasId != 0)
        {
            jogos4 = _jogoRepository.JogosEmpresas(EmpresasId);
        }

        int controle = 0;

        if(jogos1.Count > 0)
        {
            jogosfinal = jogos1;
            controle = 1;
        }

        if(jogos2.Count > 0)
        {
            if(jogosfinal.Count > 0)
            {
                jogosfinal = jogosfinal.Intersect(jogos2).ToList();
            } else {
                jogosfinal = jogos2;
            }
            controle = 1;
        }

        if(jogos3.Count > 0)
        {
            if(jogosfinal.Count > 0)
            {
                jogosfinal = jogosfinal.Intersect(jogos3).ToList();
            } else {
                jogosfinal = jogos3;
            }
            controle = 1;
        }

        if(jogos4.Count > 0)
        {
            if(jogosfinal.Count > 0)
            {
                jogosfinal = jogosfinal.Intersect(jogos4).ToList();
            } else {
                jogosfinal = jogos4;
            }
            controle = 1;
        }

        if (controle == 1)
        {
            foreach(int jogoid in jogosfinal)
            {
                foreach(Jogo jogo in jogos)
                {
                    if(jogoid == jogo.JogoId)
                    {
                        jogosfiltrados.Add(jogo);
                    }
                }
            }
        } else {
            jogosfiltrados = jogos;
        }


        generos = _generoRepository.BuscarListaCompleta();
        empresas = _empresaRepository.BuscarLista();
        tipos = _tipoRepository.BuscarLista();


        ViewBag.empresas = empresas;
        ViewBag.tipos = tipos;
        ViewBag.generos = generos;
        ViewBag.jogos = jogosfiltrados;

            return View();
    }

    [HttpGet]
    public ActionResult Perfil ()
    {
        return View();
    }

    public ActionResult Biblioteca()
    {
        string? session = HttpContext.Session.GetString("usuario");
        Usuario? usuario = JsonSerializer.Deserialize<Usuario>(session);

        List<Jogo> jogos = _jogoRepository.GetJogosUsuario(usuario.IdUsuario);
        List<DLC> complementos = _dlcRepository.BuscarListaDLC(usuario.IdUsuario);

        ViewBag.jogos = jogos;
        ViewBag.complementos = complementos;

        return View();

    }
    
}

