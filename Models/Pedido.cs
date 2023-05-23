public class Pedido
{
    public int IdPedido { get; set; }
    public EnumMeioPagamento MeioPagamento { get; set; }
    public DateTime DataCompra { get; set; }
    public decimal ValorCompra { get; set; }
    public List<Object> Produtos { get; set; }
}