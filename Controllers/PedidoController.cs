using Microsoft.AspNetCore.Mvc;

public class PedidoController : Controller
{
    IPedidoRepository pedidoRepository;
    public PedidoController(IPedidoRepository pedidoRepository)
    {
        this.pedidoRepository = pedidoRepository;
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
        return View();
    }

    [HttpPost]
    public ActionResult Criar(Pedido pedido, int idUsuario, List<int> jogosIds, List<int> complementosIds)
    {
        pedidoRepository.Criar(pedido, idUsuario, jogosIds, complementosIds);
        return RedirectToAction("Index");
    }

}