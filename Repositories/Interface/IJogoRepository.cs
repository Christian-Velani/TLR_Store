using System.Collections;

public interface IJogoRepository
{
    void CadastrarJogo (Jogo jogo);
    List<Jogo> GetAllJogos ();
    void DeleteJogo(int idJogo);
    void UpdateJogo(int idJogo);
    Jogo GetJogo(int idJogo);
}