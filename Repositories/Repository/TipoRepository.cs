using System.Data.SqlClient;

public class TipoRepository : Database, ITipoRepository
{
    public void Atualizar(Tipo tipo, int idTipo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"UPDATE TIPOS
                            SET nome = @nome
                            WHERE idTipo = @id";

        cmd.Parameters.AddWithValue("@nome", tipo.NomeTipo);
        cmd.Parameters.AddWithValue("@id", idTipo);

        cmd.ExecuteNonQuery();
    }

    public Tipo Buscar(int idTipo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT * FROM TIPOS
                            WHERE idTipo = @id";

        cmd.Parameters.AddWithValue("@id", idTipo);

        SqlDataReader reader = cmd.ExecuteReader();

        if(reader.Read())
        {
            Tipo tipo = new Tipo();
            tipo.IdTipo = Convert.ToInt32(reader["idTipo"]);
            tipo.NomeTipo = reader["nome"].ToString();

            return tipo;
        }

        return null;
    }

    public List<Tipo> BuscarLista()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT * FROM TIPOS";

        SqlDataReader reader = cmd.ExecuteReader();

        List<Tipo> tipos = new List<Tipo>();

        while(reader.Read())
        {
            Tipo tipo = new Tipo();
            tipo.IdTipo = Convert.ToInt32(reader["idTipo"]);
            tipo.NomeTipo = reader["nome"].ToString();

            tipos.Add(tipo);
        }

        return tipos;
    }

    public List<Tipo> BuscarListaJogo(int idJogo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT T.idTipo, T.nome
                            FROM TIPOS T
                            INNER JOIN JOGOS_TIPOS JT
                            ON T.idTipo = JT.tipoId
                            WHERE JT.jogoId = @id";
        
        cmd.Parameters.AddWithValue("@id", idJogo);

        SqlDataReader reader = cmd.ExecuteReader();

        List<Tipo> tipos = new List<Tipo>();

        while(reader.Read())
        {
            Tipo tipo = new Tipo();
            tipo.IdTipo = Convert.ToInt32(reader["idTipo"]);
            tipo.NomeTipo = reader["nome"].ToString();

            tipos.Add(tipo);
        }

        return tipos;
    }

    public void Cadastrar(Tipo tipo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"INSERT INTO TIPOS(nome) VALUES(@nome)";

        cmd.Parameters.AddWithValue("@nome", tipo.NomeTipo);

        cmd.ExecuteNonQuery();
    }
}