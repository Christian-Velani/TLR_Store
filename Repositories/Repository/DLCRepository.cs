using System.Data.SqlClient;
using System.Text.Json;

public class DLCRepository : Database, IDLCRepository
{
    public void Atualizar(int idDLC, DLC Complemento)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"UPDATE COMPLEMENTOS
                            SET nome = @nome, 
                                imagem = @imagem,
                                preco = @preco, 
                                descricao = @descricao
                            WHERE idComplemento = @id";
        
        cmd.Parameters.AddWithValue("@id", idDLC);
        cmd.Parameters.AddWithValue("@nome", Complemento.NomeComplemento);
        cmd.Parameters.AddWithValue("@imagem", Complemento.Imagem);
        cmd.Parameters.AddWithValue("@preco", Complemento.Preco);
        cmd.Parameters.AddWithValue("@descricao", Complemento.Descricao);

        cmd.ExecuteNonQuery();
    }

    public DLC Buscar(int idDLC)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT * FROM COMPLEMENTOS
                            WHERE idComplemento = @id";

        cmd.Parameters.AddWithValue("@id", idDLC);

        SqlDataReader reader = cmd.ExecuteReader();

        DLC complemento = new DLC();

        if(reader.Read())
        {
            byte[] imagemBytes = (byte[])reader["imagem"];

            complemento.IdComplemento = Convert.ToInt32(reader["idComplemento"]);
            complemento.NomeComplemento = reader["nome"].ToString();
            complemento.Imagem = imagemBytes;
            complemento.Preco = Convert.ToDecimal(reader["preco"]);
            complemento.Descricao = reader["descricao"].ToString();
            complemento.Status = (EnumStatus)(reader["status"]);

        }

        reader.Close();

        return complemento;
    }

    public List<DLC> BuscarListaDLC(int idUsuario)
    {
        List<int> listIds = new List<int>();
        List<DLC> complementosUsuarios = new List<DLC>();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = 
        @"SELECT complementoId from Complementos_USUARIOS
        WHERE usuarioId = @id";

        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@id",idUsuario);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            listIds.Add(Convert.ToInt32(reader["complementoId"]));
        }

        if (listIds != null)
        {
            foreach (var id in listIds)
            {
                var complemento = Buscar(id);
                complementosUsuarios.Add(complemento);
            }
            return complementosUsuarios;
        }

        return null;
    }

    public List<DLC> BuscarListaDLCJogo(int idJogo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT idComplemento, nome, imagem, preco, descricao, desconto, status
                            FROM COMPLEMENTOS
                            WHERE jogoId = @id";

        cmd.Parameters.AddWithValue("@id", idJogo);

        SqlDataReader reader = cmd.ExecuteReader();

        List<DLC> complementos = new List<DLC>();

        DLC complemento = new DLC();

        while(reader.Read())
        {
            byte[] imagemBytes = (byte[])reader["imagem"];

            complemento.IdComplemento = Convert.ToInt32(reader["idComplemento"]);
            complemento.NomeComplemento = reader["nome"].ToString();
            complemento.Imagem = imagemBytes;
            complemento.Preco = Convert.ToDecimal(reader["preco"]);
            complemento.Descricao = reader["descricao"].ToString();
            complemento.Status = (EnumStatus)(reader["status"]);

            complementos.Add(complemento);
        }

        reader.Close();

        return complementos;
    }

    public List<DLC> BuscarListaDLCJogoAtivo(int idJogo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT idComplemento, nome, imagem, preco, descricao, desconto, status
                            FROM COMPLEMENTOS
                            WHERE jogoId = @id and status = 1;";

        cmd.Parameters.AddWithValue("@id", idJogo);

        SqlDataReader reader = cmd.ExecuteReader();

        List<DLC> complementos = new List<DLC>();

        DLC complemento = new DLC();

        while(reader.Read())
        {
            byte[] imagemBytes = (byte[])reader["imagem"];

            complemento.IdComplemento = Convert.ToInt32(reader["idComplemento"]);
            complemento.NomeComplemento = reader["nome"].ToString();
            complemento.Imagem = imagemBytes;
            complemento.Preco = Convert.ToDecimal(reader["preco"]);
            complemento.Descricao = reader["descricao"].ToString();
            complemento.Status = (EnumStatus)(reader["status"]);

            complementos.Add(complemento);
        }

        reader.Close();

        return complementos;
    }

    public void Cadastrar(DLC complemento, int idJogo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"INSERT INTO COMPLEMENTOS(nome,imagem,preco,descricao,jogoId,status)
                            VALUES(@nome, @imagem, @preco, @descricao, @id, 1)";

        cmd.Parameters.AddWithValue("@nome", complemento.NomeComplemento);
        cmd.Parameters.AddWithValue("@imagem", complemento.Imagem);
        cmd.Parameters.AddWithValue("@descricao", complemento.Descricao);
        cmd.Parameters.AddWithValue("@preco", complemento.Preco);
        cmd.Parameters.AddWithValue("@id", idJogo);

        cmd.ExecuteNonQuery();
    }

    public void Desativar(int idDLC)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"UPDATE COMPLEMENTOS
                            SET status = 0
                            WHERE idComplemento = @id";

        cmd.Parameters.AddWithValue("@id", idDLC);

        cmd.ExecuteNonQuery();
    }
}