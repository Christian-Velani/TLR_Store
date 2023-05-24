using System.Collections;
using System.Data.SqlClient;

public class AdministradorRepository : Database, IAdministradorRepository
{
    private readonly IGeneroRepository _generoRepository;
    private readonly IDLCRepository    _dLCRepository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly ITipoRepository _tipoRepository;

    public AdministradorRepository(IGeneroRepository generoRepositor, IDLCRepository dLCRepository, IEmpresaRepository empresaRepository,
    ITipoRepository tipoRepository)
    {
        _generoRepository = generoRepositor;
        _dLCRepository = dLCRepository;
        _empresaRepository = empresaRepository;
        _tipoRepository = tipoRepository;
    }

    public void CadastrarJogo(Jogo jogo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"INSERT INTO JOGOS (nome,imagem,descricao,preco,desconto,
        dataLancamento,classificacaoIndicativa,requisitos,status)
        VALUES(@Nome,@Imagem,@Descricao,@Preco,@Desconto,@DataLancamento,@ClassificacaoIndicativa,
        @Requisitos,@Status)";

        cmd.Parameters.AddWithValue("@Nome",jogo.Nome);
        cmd.Parameters.AddWithValue("@Imagem",jogo.Imagem);
        cmd.Parameters.AddWithValue("@Descricao",jogo.Descricao);
        cmd.Parameters.AddWithValue("@Preco",jogo.Preco);
        cmd.Parameters.AddWithValue("@Desconto",jogo.Desconto);
        cmd.Parameters.AddWithValue("@DataLancamento",jogo.DataLancamento);
        cmd.Parameters.AddWithValue("@ClassificacaoIndicativa",jogo.ClassificacaoIndicativa);
        cmd.Parameters.AddWithValue("@Requisitos",jogo.Requisito);
        cmd.Parameters.AddWithValue("@Status",jogo.Status);

        cmd.ExecuteNonQuery();
    }

    public IList GetAllJogos ()
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
                DataLancamento = Convert.ToDateTime(reader["dataLancamento"]),
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

            jogo.JogoId = Convert.ToInt32(reader[idJogo]);
            jogo.Nome = reader["nome"].ToString();
            jogo.Imagem = imagemBytes;
            jogo.Descricao = reader["descricao"].ToString();
            jogo.Preco = Convert.ToDecimal(reader["preco"]);
            jogo.Desconto = Convert.ToInt32(reader["desconto"]);
            jogo.DataLancamento = Convert.ToDateTime(reader["dataLancamento"]);
            jogo.ClassificacaoIndicativa = Convert.ToInt32(reader["classificacaoIndicativa"]);
            jogo.Requisito = reader["requisito"].ToString();
            jogo.Status = (EnumStatus)(reader["status"]);

            cmd.ExecuteNonQuery();
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
        cmd.ExecuteNonQuery();
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
        cmd.ExecuteNonQuery();
        reader.Close();


        cmd.CommandText = @"SELECT * FROM JOGOS_GENEROS WHERE jogoId = @id4";
        cmd.Parameters.AddWithValue("@id4", jogo.JogoId);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Genero genero = _generoRepository.Buscar(Convert.ToInt32(reader["generoId"]));
            jogo.Genero.Add(genero);
        }
        cmd.ExecuteNonQuery();
        reader.Close();


        cmd.CommandText = @"SELECT * FROM JOGOS_TIPOS WHERE jogoId = @id5";
        cmd.Parameters.AddWithValue("@id5", jogo.JogoId);
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Tipo tipo = _tipoRepository.Buscar(Convert.ToInt32(reader["tipoId"]));
            jogo.Tipo.Add(tipo);
        }
        cmd.ExecuteNonQuery();
        reader.Close();

        cmd.CommandText = @"SELECT * FROM JOGOS_COMPLEMENTOS WHERE jogoId = @id6";
        cmd.Parameters.AddWithValue("@id6",jogo.JogoId);
        reader = cmd.ExecuteReader();
        while(reader.Read())
        {
            DLC complemento = _dLCRepository.Buscar(Convert.ToInt32(reader["complementoId"]));
            jogo.Complemento.Add(complemento);
        }
        cmd.ExecuteNonQuery();
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

    public void UpdateJogo (int idJogo)
    {

    }
}