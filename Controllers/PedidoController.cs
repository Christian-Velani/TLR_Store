using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
public class PedidoController : Controller
{
    IPedidoRepository pedidoRepository;
    private readonly IDLCRepository _dLCRepository;
    private readonly IJogoRepository _jogoRepository;
    public PedidoController(IPedidoRepository pedidoRepository, IDLCRepository dLCRepository, IJogoRepository jogoRepository)
    {
        this.pedidoRepository = pedidoRepository;
        _dLCRepository = dLCRepository;
        _jogoRepository = jogoRepository;
    }

    public ActionResult Index()
    {
        string? session = HttpContext.Session.GetString("usuario");
        Usuario? usuario = JsonSerializer.Deserialize<Usuario>(session);

        List<Pedido> pedidos = pedidoRepository.BuscarPorCliente(usuario.IdUsuario);
        return View(pedidos);
    }

    public ActionResult Detalhes(int idPedido)
    {
        Pedido pedido = pedidoRepository.Buscar(idPedido);
        return View(pedido);
    }

    [HttpPost]
    public ActionResult Criar(int MeioPagamento)
    {
        string? session = HttpContext.Session.GetString("carrinho");
        Carrinho? carrinho = JsonSerializer.Deserialize<Carrinho>(session);

        string? session2 = HttpContext.Session.GetString("usuario");
        Usuario? usuario = JsonSerializer.Deserialize<Usuario>(session2);


        Pedido pedido = new Pedido();
        pedido.MeioPagamento = (EnumMeioPagamento)(MeioPagamento);
        int idUsuario = usuario.IdUsuario;

        List<int> jogosIds = carrinho.idJogos;
        List<int> complementosIds = carrinho.idComplementos;

        pedidoRepository.Criar(pedido, jogosIds, complementosIds, idUsuario);
        return RedirectToAction("Home", "Usuario");
    }

}