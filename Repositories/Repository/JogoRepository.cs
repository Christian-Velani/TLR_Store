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
        cmd.CommandText = @"INSERT INTO JOGOS (nome,imagem,descricao,preco,desconto,
        dataLancamento,classificacaoIndicativa,requisitos,status)
        VALUES(@Nome,@Imagem,@Descricao,@Preco,@Desconto,@DataLancamento,@ClassificacaoIndicativa,
        @Requisitos,1)";

        cmd.Parameters.AddWithValue("@Nome",jogo.Nome);
        cmd.Parameters.AddWithValue("@Imagem",jogo.Imagem);
        cmd.Parameters.AddWithValue("@Descricao",jogo.Descricao);
        cmd.Parameters.AddWithValue("@Preco",jogo.Preco);
        cmd.Parameters.AddWithValue("@Desconto",jogo.Desconto);
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
                Desconto = Convert.ToInt32(reader["desconto"]),
                DataLancamento = reader["dataLancamento"].ToString(),
                ClassificacaoIndicativa = Convert.ToInt32(reader["classificacaoIndicativa"]),
                Requisito = reader["requisitos"].ToString(),
                Status = (EnumStatus)(reader["status"])
            });
        }

        return jogos;
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
            jogo.Desconto = Convert.ToInt32(reader["desconto"]);
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

        cmd.CommandText = @"SELECT * FROM JOGOS_EMPRESAS WHERE jogoId = @id2
                            AND tipoEmpresa = 1";

        cmd.Parameters.AddWithValue("@id2", jogo.JogoId);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Empresa empresa = _empresaRepository.Buscar(Convert.ToInt32(reader["empresaId"]));
            jogo.Desenvolvedora.Add(empresa);
        }

        reader.Close();


        cmd.CommandText = @"SELECT * FROM JOGOS_EMPRESAS WHERE jogoId = @id3
                            AND tipoEmpresa = 2";

        cmd.Parameters.AddWithValue("@id3", jogo.JogoId);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Empresa empresa = _empresaRepository.Buscar(Convert.ToInt32(reader["empresaId"]));
            jogo.Desenvolvedora.Add(empresa);
        }

        reader.Close();


        cmd.CommandText = @"SELECT * FROM JOGOS_GENEROS WHERE jogoId = @id4";
        cmd.Parameters.AddWithValue("@id4", jogo.JogoId);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Genero genero = _generoRepository.Buscar(Convert.ToInt32(reader["generoId"]));
            jogo.Genero.Add(genero);
        }

        reader.Close();


        cmd.CommandText = @"SELECT * FROM JOGOS_TIPOS WHERE jogoId = @id5";
        cmd.Parameters.AddWithValue("@id5", jogo.JogoId);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Tipo tipo = _tipoRepository.Buscar(Convert.ToInt32(reader["tipoId"]));
            jogo.Tipo.Add(tipo);
        }

        reader.Close();

        cmd.CommandText = @"SELECT * FROM JOGOS_COMPLEMENTOS WHERE jogoId = @id6";
        cmd.Parameters.AddWithValue("@id6",jogo.JogoId);
        reader = cmd.ExecuteReader();
        while(reader.Read())
        {
            DLC complemento = _dLCRepository.Buscar(Convert.ToInt32(reader["complementoId"]));
            jogo.Complemento.Add(complemento);
        }

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
        SET nome = @Nome, imagem = @Imagem, descricao = @Descricao, preco = @Preco, desconto = @Desconto, dataLancamento = @DataLancamento, classificacaoIndicativa = @ClassificacaoIndicativa, requisitos = @Requisitos
        WHERE idJogo = @id";

        cmd.Parameters.AddWithValue("@id",idJogo);
        cmd.Parameters.AddWithValue("@Nome",jogo.Nome);
        cmd.Parameters.AddWithValue("@Imagem",jogo.Imagem);
        cmd.Parameters.AddWithValue("@Descricao",jogo.Descricao);
        cmd.Parameters.AddWithValue("@Preco",jogo.Preco);
        cmd.Parameters.AddWithValue("@Desconto",jogo.Desconto);
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
    public IList GetJogosUsuario ()
    {
        Usuario? usuario = JsonSerializer.Deserialize<Usuario>("usuario");
        List<int> listIds = new List<int>();
        List<Jogo> jogosUsuarios = new List<Jogo>();
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = 
        @"SELECT jogoId JOGOS_USUARIOS
        WHERE usuarioId = @id";

        cmd.Connection = conn;
        cmd.Parameters.AddWithValue("@id",usuario.IdUsuario);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            listIds.Add(Convert.ToInt32(reader["jogoId"]));
        }

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
}