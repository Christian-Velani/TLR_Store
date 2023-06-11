public interface IUsuarioRepository
{
    Usuario? Login (string email, string senha);
    List<Usuario> ListUsuarios ();
}