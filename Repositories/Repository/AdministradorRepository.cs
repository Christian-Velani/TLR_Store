using System.Collections;
using System.Data.SqlClient;

public class AdministradorRepository : Database, IAdministradorRepository
{
    public void CadastrarJogo(Jogo jogo)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"INSERT INTO JOGOS (nome,imagem,descricao,preco,desconto
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
}