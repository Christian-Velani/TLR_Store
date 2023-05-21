using System.Collections;

public interface IAdministradorRepository
{
    void CadastrarJogo (Jogo jogo);
    IList GetAllJogos ();
}