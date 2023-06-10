using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
public class UsuarioController : Controller
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    [HttpGet]
    [Route("Usuario/Login")] 
    public ActionResult Login ()
    {
        return View();
    }

    [HttpPost]
    [Route("Usuario/Login")] 
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
        
        return RedirectToAction("Index","Jogo");
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


}