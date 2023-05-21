using Microsoft.AspNetCore.Mvc;

public class EmpresaController : Controller
{
    IEmpresaRepository empresaRepository;

    public EmpresaController(IEmpresaRepository empresaRepository)
    {
        this.empresaRepository = empresaRepository;
    }

    public ActionResult Index()
    {
        return View();
    }

    public ActionResult ListaEmpresasTipo(int idJogo, int tipo)
    {
        List<Empresa> empresas = empresaRepository.BuscarListaTipo(idJogo, tipo);
        return View(empresas);
    }

    public ActionResult ListaEmpresas()
    {
        List<Empresa> empresas = empresaRepository.BuscarLista();
        return View(empresas);
    }

    [HttpGet]
    public ActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Cadastrar(Empresa empresa)
    {
        empresaRepository.Cadastrar(empresa);
        return RedirectToAction("Index");
    }

    public ActionResult Atualizar(int id) {
        
        Empresa empresa = empresaRepository.Buscar(id);

        if(empresa != null)
            return View(empresa);
        
        return NotFound();
    }

    [HttpPost]
    public ActionResult Atualizar(int id, Empresa empresa)
    {
        empresaRepository.Atualizar(empresa, id);
        return RedirectToAction("Index");
    }


}