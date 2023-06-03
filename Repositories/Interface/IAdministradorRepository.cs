using System.Collections;

public interface IAdministradorRepository
{
    void CadastrarJogo (Jogo jogo);
    List<Jogo> GetAllJogos ();
    void DeleteJogo(int idJogo);
    void UpdateJogo(int idJogo);
    Jogo GetJogo(int idJogo);
}