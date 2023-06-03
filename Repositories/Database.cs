using System.Data.SqlClient;

public abstract class Database : IDisposable
{
    protected SqlConnection conn;

    public Database()
    {
        string connectionString = "Data Source=CHRISTIANVELANI; Initial Catalog=BD_TLRStore; Integrated Security=true; TrustServerCertificate=true";

        conn = new SqlConnection(connectionString);
        conn.Open();

        Console.WriteLine("Conexão aberta");
    }

    public void Dispose()
    {
        conn.Close();
        Console.WriteLine("Conexão fechada");
    }
}