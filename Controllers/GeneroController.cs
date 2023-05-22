using Microsoft.AspNetCore.Mvc;

public class GeneroController : Controller
{
    IGeneroRepository generoRepository;

    public GeneroController(IGeneroRepository generoRepository)
    {
        this.generoRepository = generoRepository;
    }

    public ActionResult Index()
    {
        return View();
    }

    public ActionResult ListaGenerosJogo(int idJogo)
    {
        List<Genero> generos = generoRepository.BuscarListaPorJogo(idJogo);
        return View(generos);
    }

    public ActionResult ListaGeneros()
    {
        List<Genero> generos = generoRepository.BuscarListaCompleta();
        return View(generos);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Cadastrar(Genero genero)
    {
        generoRepository.Cadastrar(genero);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Atualizar(int id)
    {
        Genero genero = generoRepository.Buscar(id);
        if(genero != null)
        {
            return View(genero);
        }

        return NotFound();
        
    }

    [HttpPost]
    public ActionResult Atualizar(int id, Genero genero)
    {
        generoRepository.Atualizar(id, genero);
        return RedirectToAction("Index");
    }
}