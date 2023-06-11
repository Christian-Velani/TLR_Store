public interface IUsuarioRepository
{
    Usuario? Login (string email, string senha);
    void Cadastro (Usuario usuario);
    List<Usuario> ListUsuarios ();
}