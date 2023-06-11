public interface IUsuarioRepository
{
<<<<<<< HEAD
    Usuario? Login (string email, string senha);
=======
    void Cadastro (Usuario usuario);
    Usuario? Login (string login, string senha);
>>>>>>> 2bce56de42c2285ae5f32ee9b32f7b6a34e38b66
    List<Usuario> ListUsuarios ();
}