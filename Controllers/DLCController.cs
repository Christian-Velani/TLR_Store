using Microsoft.AspNetCore.Mvc;

public class DLCController : Controller
{
    private readonly IDLCRepository _dlcRepository;
    
    public DLCController(IDLCRepository dlcRepository)
    {
        this._dlcRepository = dlcRepository;
    }

    public ActionResult Index()
    {
        return View();
    }

    public ActionResult ListaDLCs()
    {
        List<DLC> complementos = _dlcRepository.BuscarListaDLC();
        return View(complementos);
    }

    public ActionResult ListaDLCsJogo(int idJogo)
    {
        List<DLC> complementos = _dlcRepository.BuscarListaDLCJogo(idJogo);
        return View(complementos);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        return View();
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
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public ActionResult Atualizar(int id)
    {
        DLC complemento = _dlcRepository.Buscar(id);
        if(complemento != null)
        {
            return View(complemento);
        }

        return NotFound();
    }

    [HttpPost]
    public ActionResult Atualizar(DLC complemento, int idDLC)
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
            DLC complementoRegistrado = _dlcRepository.Buscar(idDLC); 
            complemento.Imagem = complementoRegistrado.Imagem;
        }
        _dlcRepository.Atualizar(idDLC, complemento);
        return RedirectToAction("Index");
    }

    public ActionResult Desativar(int idDLC)
    {
        _dlcRepository.Desativar(idDLC);
        return RedirectToAction("Index");
    }
}