public interface IPedidoRepository
{
    List<Pedido> BuscarPorCliente(int idUsuario);
    Pedido Buscar(int idPedido);
    void Criar(Pedido pedido, List<int> jogosIds, List<int> complementosIds);
}