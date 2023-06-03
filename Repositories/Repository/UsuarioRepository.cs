public class UsuarioRepository : IUsuarioRepository
{
    public List<Usuario> ListUsuarios ()
    {
        return null; //IMPLEMENTAR ISSO AQUI
    }

    public Usuario? Login (string email, string senha)
    {
        var usuarios = ListUsuarios();
        var currentUser = usuarios.SingleOrDefault(user => user.Email == email && user.Senha == senha);

        return currentUser;
    }
}