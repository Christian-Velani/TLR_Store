using System.Data.SqlClient;

public class UsuarioRepository : Database, IUsuarioRepository
{
    public List<Usuario> ListUsuarios ()
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT * FROM USUARIOS";
        
        SqlDataReader reader = cmd.ExecuteReader();
        var usuarios = new List<Usuario>();

        while (reader.Read())
        {
            byte[] imagemBytes = (byte[])reader["icone"];

            usuarios.Add(new Usuario(){
                IdUsuario = Convert.ToInt32(reader["idUsuario"]),
                NomeUsuario = reader["nome"].ToString(),
                Nick = reader["nick"].ToString(),
                Email = reader["email"].ToString(),
                Senha = reader["senha"].ToString(),
                Icone = imagemBytes
            });
        }

        return usuarios;
    }

    public Usuario? Login (string login, string senha)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = @"SELECT * FROM USUARIOS
                            WHERE (email = @Email or nick = @Nick) and senha = @Senha";
                 
        cmd.Parameters.AddWithValue("@Email",login);
        cmd.Parameters.AddWithValue("@Nick",login);
        cmd.Parameters.AddWithValue("@Senha",senha);

        SqlDataReader reader = cmd.ExecuteReader();
        var usuario = new Usuario();

        if(reader.Read())
        {
            if(reader["icone"] != DBNull.Value)
            {
                byte[] imagemBytes = (byte[])reader["icone"];
                usuario.Icone = imagemBytes;
            }
            usuario.Email = reader["email"].ToString();
            usuario.Senha = reader["senha"].ToString();
            usuario.Nick = reader["nick"].ToString();
            usuario.Status = (EnumStatus)reader["status"];
            usuario.IdUsuario = Convert.ToInt32(reader["idUsuario"]);
            usuario.TipoUsuario = (EnumTipoUsuario)reader["tipo"];
            
            return usuario;
        }

        return null;
    }

    public void Cadastro (Usuario usuario)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = 
        @"INSERT INTO USUARIOS (icone,nome,nick,senha,email,status,tipo)
        VALUES (@Icone,@Nome,@Nick,@Senha,@Email,@Status,0)";

        cmd.Parameters.AddWithValue("@Icone",usuario.Icone);
        cmd.Parameters.AddWithValue("@Nome",usuario.NomeUsuario);
        cmd.Parameters.AddWithValue("@Nick",usuario.Nick);
        cmd.Parameters.AddWithValue("@Senha",usuario.Senha);
        cmd.Parameters.AddWithValue("@Email",usuario.Email);
        cmd.Parameters.AddWithValue("@Status",usuario.Status);

        cmd.ExecuteNonQuery();
    }
}