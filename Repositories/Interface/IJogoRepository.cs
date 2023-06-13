using System.Collections;

public interface IJogoRepository
{
    void CadastrarJogo (Jogo jogo, List<int> generos, List<int> tipos, List<int> desenvolvedoras, List<int> distribuidoras);
    List<Jogo> GetAllJogos ();
    List<Jogo> GetAllJogosAtivos ();
    void DeleteJogo(int idJogo);
    void UpdateJogo (int idJogo, Jogo jogo, List<int> generos, List<int> tipos, List<int> desenvolvedoras, List<int> distribuidoras);
    Jogo GetJogo(int idJogo);
    List<Jogo> GetJogosUsuario (int idUsuario);
    List<int>? SearchJogosNome (string? search);
    List<int>? JogosGeneros (int idGenero);
    List<int>? JogosTipos (int idTipo);
    List<int>? JogosEmpresas (int idEmpresa);
    List<Jogo>? SearchJogosNome2(string? search);
}