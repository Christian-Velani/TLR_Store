using Microsoft.AspNetCore.Mvc;

public class TipoController : Controller
{
    ITipoRepository tipoRepository;
    public TipoController(ITipoRepository tipoRepository)
    {
        this.tipoRepository = tipoRepository;
    }

    public ActionResult Index()
    {
        return View();
    }

    public ActionResult ListaTiposJogo(int idJogo)
    {
        List<Tipo> tipos = tipoRepository.BuscarListaJogo(idJogo);
        return View(tipos);
    }

    public ActionResult ListaTipos()
    {
        List<Tipo> tipos = tipoRepository.BuscarLista();
        return View(tipos);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Cadastrar(Tipo tipo)
    {
        tipoRepository.Cadastrar(tipo);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Atualizar(int id)
    {
        Tipo tipo = tipoRepository.Buscar(id);
        if(tipo != null)
        {
            return View(tipo);
        }

        return NotFound();        
    }

    [HttpPost]
    public ActionResult Atualizar(int id, Tipo tipo)
    {
        tipoRepository.Atualizar(tipo, id);
        return RedirectToAction("Index");
    }
}