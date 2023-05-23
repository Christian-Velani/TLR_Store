using System.Data.SqlClient;

public class PedidoRepository : Database, IPedidoRepository
{
    IDLCRepository dlcRepository;

    public PedidoRepository(IDLCRepository dlcRepository)
    {
        this.dlcRepository = dlcRepository;
    }
    public List<Pedido> BuscarPorCliente(int idUsuario)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT * FROM PEDIDOS WHERE usuarioId = @id";

        cmd.Parameters.AddWithValue("@id", idUsuario);

        SqlDataReader reader = cmd.ExecuteReader();

        List<Pedido> pedidos = new List<Pedido>();

        while(reader.Read())
        {
            Pedido pedido = new Pedido();
            pedido.IdPedido = Convert.ToInt32(reader["idPedido"]);
            pedido.MeioPagamento = (EnumMeioPagamento)(reader["meioPagamento"]);
            pedido.DataCompra = Convert.ToDateTime(reader["dataCompra"]);
            pedido.ValorCompra = Convert.ToDecimal(reader["valorCompra"]);

            pedidos.Add(pedido);
        }

        return pedidos;
    }

    public Pedido Buscar(int idPedido)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT * FROM PEDIDOS WHERE idPedido = @id1";

        cmd.Parameters.AddWithValue("@id1", idPedido);

        SqlDataReader reader = cmd.ExecuteReader();

        Pedido pedido = new Pedido();

        if(reader.Read())
        {
            pedido.IdPedido = Convert.ToInt32(reader["idPedido"]);
            pedido.MeioPagamento = (EnumMeioPagamento)(reader["meioPagamento"]);
            pedido.DataCompra = Convert.ToDateTime(reader["dataCompra"]);
            pedido.ValorCompra = Convert.ToDecimal(reader["valorCompra"]);
        }

        reader.Close();

        pedido.Produtos = new List<Object>();

        if(pedido != null)
        {    
            cmd.CommandText = @"SELECT * FROM PRODUTOS_PEDIDOS WHERE pedidoId = @id2";

            cmd.Parameters.AddWithValue("@id2", pedido.IdPedido);

            reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                if(reader["jogoId"] != DBNull.Value)
                {

                }
                else if(reader["complementoId"] != DBNull.Value)
                {
                    DLC complemento = dlcRepository.Buscar(Convert.ToInt32(reader["complementoId"]));
                    pedido.Produtos.Add(complemento);
                }
            }

            return pedido;
        }

        return null;
    }

    public void Criar(Pedido pedido, int idUsuario, List<int> jogosIds, List<int> complementosIds)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"INSERT INTO PEDIDOS(meioPagamento, dataCompra, usuarioId) 
                            VALUES(@meioPagamento, @dataCompra, @usuarioId)";

        cmd.Parameters.AddWithValue("@meioPagamento", pedido.MeioPagamento);
        cmd.Parameters.AddWithValue("@dataCompra", pedido.DataCompra);
        cmd.Parameters.AddWithValue("@usuarioId", idUsuario);

        cmd.ExecuteNonQuery();

        foreach(int id in jogosIds)
        {
            cmd.CommandText = "INSERT INTO PRODUTOS_PEDIDOS(pedidoId, jogoId) VALUES(@pedidoId, @jogoId)";

            cmd.Parameters.AddWithValue("@pedidoId", pedido.IdPedido);
            cmd.Parameters.AddWithValue("@jogoId", id);

            cmd.ExecuteNonQuery();
        }

        foreach(int id in complementosIds)
        {
            cmd.CommandText = "INSERT INTO PRODUTOS_PEDIDOS(pedidoId, complementoId) VALUES(@pedidoId, @complementoId)";

            cmd.Parameters.AddWithValue("@pedidoId", pedido.IdPedido);
            cmd.Parameters.AddWithValue("@complementoId", id);

            cmd.ExecuteNonQuery();
        }

        cmd.CommandText = @"UPDATE PEDIDOS
                            SET valorCompra = (
                                SELECT COALESCE(SUM(ISNULL(j.preco, 0) + ISNULL(c.preco, 0)), 0)
                                FROM PRODUTOS_PEDIDOS pp
                                LEFT JOIN JOGOS j ON pp.jogoId = j.idJogo
                                LEFT JOIN COMPLEMENTOS c ON pp.complementoId = c.idComplemento
                                WHERE pp.pedidoId = @idPedido
                            )
                            WHERE idPedido = @idPedido;)";

        cmd.Parameters.AddWithValue("@idpedido", pedido.IdPedido);

        cmd.ExecuteNonQuery();
    }
}