using System.Data.SqlClient;

public class EmpresaRepository : Database, IEmpresaRepository
{
    public void Atualizar(Empresa empresa, int id)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"UPDATE EMPRESAS 
                            SET nome = @nome
                            WHERE idEmpresa = @id";

        cmd.Parameters.AddWithValue("@nome", empresa.NomeEmpresa);
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();

    }

    public Empresa Buscar(int empresaId)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT idEmpresa, nome FROM EMPRESAS WHERE idEmpresa = @id";

        cmd.Parameters.AddWithValue("@id", empresaId);

        SqlDataReader reader = cmd.ExecuteReader();

        if(reader.Read())
        {
            Empresa empresa = new Empresa();
            empresa.IdEmpresa = Convert.ToInt32(reader["idEmpresa"]);
            empresa.NomeEmpresa = reader["nome"].ToString();

            return empresa;
        }

        return null;
    }

    public List<Empresa> BuscarLista()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT * FROM EMPRESAS";

        List<Empresa> empresas = new List<Empresa>();

        SqlDataReader reader = cmd.ExecuteReader();

        while(reader.Read())
        {
            Empresa empresa = new Empresa();
            empresa.IdEmpresa = Convert.ToInt32(reader["idEmpresa"]);
            empresa.NomeEmpresa = reader["nome"].ToString();

            empresas.Add(empresa);
        }

        return empresas;
    }

    public List<Empresa> BuscarListaTipo(int jogoId, int tipo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT E.idEmpresa, E.nome 
                            FROM EMPRESAS E 
                            INNER JOIN JOGOS_EMPRESAS JE
                            ON E.idEmpresa = JE.empresaId
                            WHERE JE.jogoId = @jogoid and JE.tipoEmpresa = @tipo";

        cmd.Parameters.AddWithValue("@jogoId", jogoId);
        cmd.Parameters.AddWithValue("@tipo", tipo);

        List<Empresa> empresas = new List<Empresa>();

        SqlDataReader reader = cmd.ExecuteReader();

        while(reader.Read())
        {
            Empresa empresa = new Empresa();
            empresa.IdEmpresa = Convert.ToInt32(reader["idEmpresa"]);
            empresa.NomeEmpresa = reader["nome"].ToString();
            
            empresas.Add(empresa);
        }

        return empresas;
    }

    public void Cadastrar(Empresa empresa)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"INSERT INTO EMPRESAS(nome)
                            VALUES(@nome)";
        
        cmd.Parameters.AddWithValue("@nome", empresa.NomeEmpresa);

        cmd.ExecuteNonQuery();
    }
}