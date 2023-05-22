using System.Data.SqlClient;

public class GeneroRepository : Database, IGeneroRepository
{
    public void Atualizar(int idGenero, Genero genero)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"UPDATE GENEROs
                            SET nome = @nome
                            WHERE idGenero = @id";

        cmd.Parameters.AddWithValue("@nome", genero.NomeGenero);
        cmd.Parameters.AddWithValue("@id", idGenero);

        cmd.ExecuteNonQuery();
    }

    public Genero Buscar(int idGenero)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT idGenero, nome
                            FROM GENEROS
                            WHERE idGenero = @id";
        
        cmd.Parameters.AddWithValue("@id", idGenero);

        SqlDataReader reader = cmd.ExecuteReader();

        if(reader.Read())
        {
            Genero genero = new Genero();
            genero.IdGenero = Convert.ToInt32(reader["idGenero"]);
            genero.NomeGenero = reader["nome"].ToString();

            return genero;
        }

        return null;
    }

    public List<Genero> BuscarListaCompleta()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT * FROM GENEROS";

        List<Genero> generos = new List<Genero>();

        SqlDataReader reader = cmd.ExecuteReader();

        while(reader.Read())
        {
            Genero genero = new Genero();
            genero.IdGenero = Convert.ToInt32(reader["idGenero"]);
            genero.NomeGenero = reader["nome"].ToString();

            generos.Add(genero);
        }

        return generos;
    }

    public List<Genero> BuscarListaPorJogo(int idJogo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT G.idGenero, G.nome
                            FROM GENEROS G
                            INNER JOIN JOGOS_GENEROS JG
                            ON G.idGenero = JG.generoId
                            WHERE JG.jogoId = @id";

        cmd.Parameters.AddWithValue("@id", idJogo);

        SqlDataReader reader = cmd.ExecuteReader();

        List<Genero> generos = new List<Genero>();

        while(reader.Read())
        {
            Genero genero = new Genero();
            genero.IdGenero = Convert.ToInt32(reader["idGenero"]);
            genero.NomeGenero = reader["nome"].ToString();

            generos.Add(genero);
        }

        return generos;
    }

    public void Cadastrar(Genero genero)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"INSERT INTO GENEROS(nome) VALUES(@nome)";

        cmd.Parameters.AddWithValue("@nome", genero.NomeGenero);

        cmd.ExecuteNonQuery();
    }
}