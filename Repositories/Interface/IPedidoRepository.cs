public interface IPedidoRepository
{
    List<Pedido> BuscarPorCliente(int idUsuario);
    Pedido Buscar(int idPedido);
    void Criar(Pedido pedido, int idUsuario, List<int> jogosIds, List<int> complementosIds);
}