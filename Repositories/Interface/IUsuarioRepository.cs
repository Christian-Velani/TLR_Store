public interface IUsuarioRepository
{
    void Cadastro (Usuario usuario);
    Usuario? Login (string login, string senha);
    List<Usuario> ListUsuarios ();
}