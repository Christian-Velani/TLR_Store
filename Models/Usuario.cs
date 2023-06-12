public class Usuario
{
    public int IdUsuario { get; set; }
    public byte[] Icone { get; set; }
    public string NomeUsuario { get; set; }
    public string Nick { get; set; }
    public string Senha { get; set; }
    public string Email { get; set; }
    public EnumStatus Status { get; set; }
    public List<Jogo> JogosAdquiridos { get; set; }
    public List<DLC> ComplementosAdquiridos { get; set; }
    public List<Pedido> HistoricoCompras { get; set; }
    public EnumTipoUsuario TipoUsuario { get; set; }
}