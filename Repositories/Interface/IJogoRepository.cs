using System.Collections;

public interface IJogoRepository
{
    void CadastrarJogo (Jogo jogo, List<int> generos, List<int> tipos, List<int> desenvolvedoras, List<int> distribuidoras);
    List<Jogo> GetAllJogos ();
    void DeleteJogo(int idJogo);
    void UpdateJogo(int idJogo);
    Jogo GetJogo(int idJogo);
    IList GetJogosUsuario ();
}