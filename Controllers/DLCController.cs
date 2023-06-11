using Microsoft.AspNetCore.Mvc;

public class DLCController : Controller
{
    IDLCRepository dlcRepository;
    
    public DLCController(IDLCRepository dlcRepository)
    {
        this.dlcRepository = dlcRepository;
    }

    public ActionResult Index()
    {
        return View();
    }

    public ActionResult ListaDLCs()
    {
        List<DLC> complementos = dlcRepository.BuscarListaDLC();
        return View(complementos);
    }

    public ActionResult ListaDLCsJogo(int idJogo)
    {
        List<DLC> complementos = dlcRepository.BuscarListaDLCJogo(idJogo);
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
        dlcRepository.Cadastrar(complemento, idJogo);
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public ActionResult Atualizar(int id)
    {
        DLC complemento = dlcRepository.Buscar(id);
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
            DLC complementoRegistrado = dlcRepository.Buscar(idDLC); 
            complemento.Imagem = complementoRegistrado.Imagem;
        }
        dlcRepository.Atualizar(idDLC, complemento);
        return RedirectToAction("Index");
    }

    public ActionResult Desativar(int idDLC)
    {
        dlcRepository.Desativar(idDLC);
        return RedirectToAction("Index");
    }
}