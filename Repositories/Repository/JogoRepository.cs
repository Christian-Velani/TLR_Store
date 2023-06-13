using System.Collections;
using System.Data.SqlClient;
using System.Text.Json;

public class JogoRepository : Database, IJogoRepository
{
    private readonly IGeneroRepository _generoRepository;
    private readonly IDLCRepository    _dLCRepository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly ITipoRepository _tipoRepository;

    public JogoRepository(IGeneroRepository generoRepositor, IDLCRepository dLCRepository, IEmpresaRepository empresaRepository,
    ITipoRepository tipoRepository)
    {
        _generoRepository = generoRepositor;
        _dLCRepository = dLCRepository;
        _empresaRepository = empresaRepository;
        _tipoRepository = tipoRepository;
    }

    public void CadastrarJogo(Jogo jogo, List<int> generos, List<int> tipos, List<int> desenvolvedoras, List<int> distribuidoras)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = @"SELECT COUNT (nome)
                            FROM JOGOS
                            WHERE nome = @Nome";
        cmd.Parameters.AddWithValue("@Nome",jogo.Nome);
        int countName = (int)cmd.ExecuteScalar();

        cmd.Parameters.Clear();

        if (countName == 0)
        {
            cmd.CommandText = @"INSERT INTO JOGOS (nome,imagem,descricao,preco,
            dataLancamento,classificacaoIndicativa,requisitos,status)
            VALUES(@Nome,@Imagem,@Descricao,@Preco,@DataLancamento,@ClassificacaoIndicativa,
            @Requisitos,1)";


            cmd.Parameters.AddWithValue("@Nome",jogo.Nome);
            cmd.Parameters.AddWithValue("@Imagem",jogo.Imagem);
            cmd.Parameters.AddWithValue("@Descricao",jogo.Descricao);
            cmd.Parameters.AddWithValue("@Preco",jogo.Preco);
            cmd.Parameters.AddWithValue("@DataLancamento",jogo.DataLancamento);
            cmd.Parameters.AddWithValue("@ClassificacaoIndicativa",jogo.ClassificacaoIndicativa);
            cmd.Parameters.AddWithValue("@Requisitos",jogo.Requisito);

            cmd.ExecuteNonQuery();

            cmd.CommandText = @"SELECT TOP 1 idJogo FROM JOGOS
                                ORDER BY idJogo DESC";

            SqlDataReader reader = cmd.ExecuteReader();

            if(reader.Read())
            {
                jogo.JogoId = Convert.ToInt32(reader["idJogo"]);
            }

            reader.Close();

            foreach(int id in tipos)
            {
                cmd.CommandText = @"INSERT INTO JOGOS_TIPOS (jogoId, tipoId) VALUES (@jogoId, @tipoId)";

                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@jogoId", jogo.JogoId);
                cmd.Parameters.AddWithValue("@tipoId", id);

                cmd.ExecuteNonQuery();
            }

            foreach(int id in generos)
            {
                cmd.CommandText = @"INSERT INTO JOGOS_GENEROS (jogoId, generoId) VALUES (@jogoId, @generoId)";

                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@jogoId", jogo.JogoId);
                cmd.Parameters.AddWithValue("@generoId", id);

                cmd.ExecuteNonQuery();
            }

            foreach(int id in desenvolvedoras)
            {
                cmd.CommandText = @"INSERT INTO JOGOS_EMPRESAS (jogoId, empresaId, tipoEmpresa) VALUES (@jogoId, @empresaId, 1)";

                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@jogoId", jogo.JogoId);
                cmd.Parameters.AddWithValue("@empresaId", id);

                cmd.ExecuteNonQuery();
            }

            foreach(int id in distribuidoras)
            {
                cmd.CommandText = @"INSERT INTO JOGOS_EMPRESAS (jogoId, empresaId, tipoEmpresa) VALUES (@jogoId, @empresaId, 2)";

                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@jogoId", jogo.JogoId);
                cmd.Parameters.AddWithValue("@empresaId", id);

                cmd.ExecuteNonQuery();
            }             
        }
    }

    public List<Jogo> GetAllJogos ()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT * FROM JOGOS";
        
        SqlDataReader reader = cmd.ExecuteReader();
        var jogos = new List<Jogo>();

        while (reader.Read())
        {
            byte[] imagemBytes = (byte[])reader["imagem"];

            jogos.Add(new Jogo(){
                JogoId = Convert.ToInt32(reader["idJogo"]),
                Nome = reader["nome"].ToString(),
                Imagem =  imagemBytes,
                Descricao = reader["descricao"].ToString(),
                Preco = Convert.ToDecimal(reader["preco"]),
                DataLancamento = reader["dataLancamento"].ToString(),
                ClassificacaoIndicativa = Convert.ToInt32(reader["classificacaoIndicativa"]),
                Requisito = reader["requisitos"].ToString(),
                Status = (EnumStatus)(reader["status"])
            });
        }

        reader.Close();

        List<Jogo> jogosFinal = new List<Jogo>();

        foreach(Jogo jogo in jogos)
        {
            jogosFinal.Add(GetJogo(jogo.JogoId));
        }

        return jogosFinal;
    }

    public List<Jogo> GetAllJogosAtivos ()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT * FROM JOGOS WHERE status = 1";
        
        SqlDataReader reader = cmd.ExecuteReader();
        var jogos = new List<Jogo>();

        while (reader.Read())
        {
            byte[] imagemBytes = (byte[])reader["imagem"];

            jogos.Add(new Jogo(){
                JogoId = Convert.ToInt32(reader["idJogo"]),
                Nome = reader["nome"].ToString(),
                Imagem =  imagemBytes,
                Descricao = reader["descricao"].ToString(),
                Preco = Convert.ToDecimal(reader["preco"]),
                DataLancamento = reader["dataLancamento"].ToString(),
                ClassificacaoIndicativa = Convert.ToInt32(reader["classificacaoIndicativa"]),
                Requisito = reader["requisitos"].ToString(),
                Status = (EnumStatus)(reader["status"])
            });
        }

        reader.Close();

        List<Jogo> jogosFinal = new List<Jogo>();

        foreach(Jogo jogo in jogos)
        {
            jogosFinal.Add(GetJogo(jogo.JogoId));
        }

        return jogosFinal;
    }

    public Jogo GetJogo (int idJogo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT * FROM JOGOS
                            WHERE idJogo = @id";
                 
        cmd.Parameters.AddWithValue("@id", idJogo);

        SqlDataReader reader = cmd.ExecuteReader();
        var jogo = new Jogo();

        if(reader.Read())
        {
            byte[] imagemBytes = (byte[])reader["imagem"];

            jogo.JogoId = Convert.ToInt32(reader["idJogo"]);
            jogo.Nome = reader["nome"].ToString();
            jogo.Imagem = imagemBytes;
            jogo.Descricao = reader["descricao"].ToString();
            jogo.Preco = Convert.ToDecimal(reader["preco"]);
            jogo.DataLancamento = reader["dataLancamento"].ToString();
            jogo.ClassificacaoIndicativa = Convert.ToInt32(reader["classificacaoIndicativa"]);
            jogo.Requisito = reader["requisitos"].ToString();
            jogo.Status = (EnumStatus)(reader["status"]);
        }

        reader.Close();

        jogo.Desenvolvedora = new List<Empresa>();
        jogo.Distribuidora = new List<Empresa>();
        jogo.Genero = new List<Genero>();
        jogo.Tipo = new List<Tipo>();
        jogo.Complemento = new List<DLC>();

        jogo.Genero = _generoRepository.BuscarListaPorJogo(jogo.JogoId);

        jogo.Tipo = _tipoRepository.BuscarListaJogo(jogo.JogoId);

        jogo.Desenvolvedora = _empresaRepository.BuscarListaTipo(jogo.JogoId, 1);

        jogo.Distribuidora = _empresaRepository.BuscarListaTipo(jogo.JogoId, 2);

        jogo.Complemento = _dLCRepository.BuscarListaDLCJogo(jogo.JogoId);

        reader.Close();

        return jogo;
    }
    public void DeleteJogo (int idJogo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = @"UPDATE JOGOS
                            SET status = 0 
                            WHERE idJogo = @id";

        cmd.Parameters.AddWithValue("@id",idJogo);

        cmd.ExecuteNonQuery();
    }
    public void UpdateJogo (int idJogo, Jogo jogo, List<int> generos, List<int> tipos, List<int> desenvolvedoras, List<int> distribuidoras)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"
        UPDATE JOGOS
        SET nome = @Nome, imagem = @Imagem, descricao = @Descricao, preco = @Preco, dataLancamento = @DataLancamento, classificacaoIndicativa = @ClassificacaoIndicativa, requisitos = @Requisitos
        WHERE idJogo = @id";

        cmd.Parameters.AddWithValue("@id",idJogo);
        cmd.Parameters.AddWithValue("@Nome",jogo.Nome);
        cmd.Parameters.AddWithValue("@Imagem",jogo.Imagem);
        cmd.Parameters.AddWithValue("@Descricao",jogo.Descricao);
        cmd.Parameters.AddWithValue("@Preco",jogo.Preco);
        cmd.Parameters.AddWithValue("@DataLancamento",jogo.DataLancamento);
        cmd.Parameters.AddWithValue("@ClassificacaoIndicativa",jogo.ClassificacaoIndicativa);
        cmd.Parameters.AddWithValue("@Requisitos",jogo.Requisito);

        cmd.ExecuteNonQuery();

        cmd.CommandText = @"DELETE FROM JOGOS_TIPOS WHERE jogoId = @id";

        cmd.ExecuteNonQuery();

        cmd.CommandText = @"DELETE FROM JOGOS_GENEROS WHERE jogoId = @id";

        cmd.ExecuteNonQuery();

        cmd.CommandText = @"DELETE FROM JOGOS_EMPRESAS WHERE jogoId = @id";

        cmd.ExecuteNonQuery();

        jogo.JogoId = idJogo;

        foreach(int id in tipos)
        {
            cmd.CommandText = @"INSERT INTO JOGOS_TIPOS (jogoId, tipoId) VALUES (@jogoId, @tipoId)";

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@jogoId", jogo.JogoId);
            cmd.Parameters.AddWithValue("@tipoId", id);

            cmd.ExecuteNonQuery();
        }

        foreach(int id in generos)
        {
            cmd.CommandText = @"INSERT INTO JOGOS_GENEROS (jogoId, generoId) VALUES (@jogoId, @generoId)";

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@jogoId", jogo.JogoId);
            cmd.Parameters.AddWithValue("@generoId", id);

            cmd.ExecuteNonQuery();
        }

        foreach(int id in desenvolvedoras)
        {
            cmd.CommandText = @"INSERT INTO JOGOS_EMPRESAS (jogoId, empresaId, tipoEmpresa) VALUES (@jogoId, @empresaId, 1)";

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@jogoId", jogo.JogoId);
            cmd.Parameters.AddWithValue("@empresaId", id);

            cmd.ExecuteNonQuery();
        }

        foreach(int id in distribuidoras)
        {
            cmd.CommandText = @"INSERT INTO JOGOS_EMPRESAS (jogoId, empresaId, tipoEmpresa) VALUES (@jogoId, @empresaId, 2)";

            cmd.Parameters.Clear();

            cmd.Parameters.AddWithValue("@jogoId", jogo.JogoId);
            cmd.Parameters.AddWithValue("@empresaId", id);

            cmd.ExecuteNonQuery();
        }                          
    }
    public List<Jogo> GetJogosUsuario (int idUsuario)
    {
        List<int> listIds = new List<int>();
        List<Jogo> jogosUsuarios = new List<Jogo>();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = 
        @"SELECT jogoId from JOGOS_USUARIOS
        WHERE usuarioId = @id";

        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@id", idUsuario);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            listIds.Add(Convert.ToInt32(reader["jogoId"]));
        }

        reader.Close();

        if (listIds != null)
        {
            foreach (var id in listIds)
            {
                var jogo = GetJogo(id);
                jogosUsuarios.Add(jogo);
            }
            return jogosUsuarios;
        }

        return null;
    }

    public List<int>? SearchJogosNome(string? search)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT * FROM JOGOS
                            WHERE nome LIKE '%' + @search + '%'";
        cmd.Parameters.Clear();

        cmd.Parameters.AddWithValue("@search", search);

        SqlDataReader reader = cmd.ExecuteReader();

        List<int> jogos = new List<int>();

        while(reader.Read())
        {
            jogos.Add(Convert.ToInt32(reader["idJogo"]));
        }

        reader.Close();

        return jogos;
    }

    public List<Jogo>? SearchJogosNome2(string? search)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT * FROM JOGOS
                            WHERE nome LIKE '%' + @search + '%'";
        cmd.Parameters.Clear();

        cmd.Parameters.AddWithValue("@search", search);

        SqlDataReader reader = cmd.ExecuteReader();

        List<int> jogos = new List<int>();

        while(reader.Read())
        {
            jogos.Add(Convert.ToInt32(reader["idJogo"]));
        }

        reader.Close();

        List<Jogo> jogosFinal = new List<Jogo>();

        foreach (int idJogo in jogos)
        {
            jogosFinal.Add(GetJogo(idJogo));
        }

        return jogosFinal;
    }

    public List<int>? JogosGeneros(int idGenero)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT j.*
                            FROM JOGOS j
                            INNER JOIN JOGOS_GENEROS jg ON j.idJogo = jg.jogoId
                            WHERE jg.generoId = @id";

        cmd.Parameters.Clear();

        cmd.Parameters.AddWithValue("@id", idGenero);

        SqlDataReader reader = cmd.ExecuteReader();

        List<int> jogos = new List<int>();

        while(reader.Read())
        {
            jogos.Add(Convert.ToInt32(reader["idJogo"]));
        }

        reader.Close();

        return jogos;
    }

    public List<int>? JogosEmpresas(int idEmpresa)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT j.*
                            FROM JOGOS j
                            INNER JOIN JOGOS_EMPRESAS je ON j.idJogo = je.jogoId
                            WHERE je.empresaId = @id";

        cmd.Parameters.Clear();

        cmd.Parameters.AddWithValue("@id", idEmpresa);

        SqlDataReader reader = cmd.ExecuteReader();

        List<int> jogos = new List<int>();

        while(reader.Read())
        {
            jogos.Add(Convert.ToInt32(reader["idJogo"]));
        }

        reader.Close();

        return jogos;
    }

    public List<int>? JogosTipos(int idTipo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT j.*
                            FROM JOGOS j
                            INNER JOIN JOGOS_Tipos jt ON j.idJogo = jt.jogoId
                            WHERE jt.tipoId = @id";

        cmd.Parameters.Clear();

        cmd.Parameters.AddWithValue("@id", idTipo);

        SqlDataReader reader = cmd.ExecuteReader();

        List<int> jogos = new List<int>();

        while(reader.Read())
        {
            jogos.Add(Convert.ToInt32(reader["idJogo"]));
        }

        reader.Close();

        return jogos;
    }

    public List<Jogo>? SearchJogos(string? search)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT * FROM JOGOS
                            WHERE nome LIKE '%' + @search + '%'";
        cmd.Parameters.Clear();

        cmd.Parameters.AddWithValue("@search", search);

        SqlDataReader reader = cmd.ExecuteReader();

        List<int> jogos = new List<int>();

        while(reader.Read())
        {
            jogos.Add(Convert.ToInt32(reader["idJogo"]));
        }

        reader.Close();
        
        List<Jogo> jogosfiltrados = new List<Jogo>();

        foreach(int id in jogos)
        {
            jogosfiltrados.Add(GetJogo(id));
        }

        return jogosfiltrados;
    }
}