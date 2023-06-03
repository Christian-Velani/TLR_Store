using Microsoft.AspNetCore.Mvc;

public class PedidoController : Controller
{
    IPedidoRepository pedidoRepository;
    private readonly IDLCRepository _dLCRepository;
    private readonly IAdministradorRepository _administradorRepository;
    public PedidoController(IPedidoRepository pedidoRepository, IDLCRepository dLCRepository, IAdministradorRepository administradorRepository)
    {
        this.pedidoRepository = pedidoRepository;
        _dLCRepository = dLCRepository;
        _administradorRepository = administradorRepository;
    }

    public ActionResult Index()
    {
        List<Pedido> pedidos = pedidoRepository.BuscarPorCliente(1);
        return View(pedidos);
    }

    public ActionResult Detalhes(int idPedido)
    {
        Pedido pedido = pedidoRepository.Buscar(idPedido);
        return View(pedido);
    }

    [HttpGet]
    public ActionResult Criar()
    {
        List<DLC> complementos = new List<DLC>();
        complementos = _dLCRepository.BuscarListaDLC();
        List<Jogo> jogos = new List<Jogo>();
        jogos = _administradorRepository.GetAllJogos();

        ViewBag.complementos = complementos;
        ViewBag.jogos = jogos;

        return View();
    }

    [HttpPost]
    public ActionResult Criar(Pedido pedido, List<int> jogosIds, List<int> complementosIds)
    {
        pedidoRepository.Criar(pedido, jogosIds, complementosIds);
        return RedirectToAction("Index");
    }

}