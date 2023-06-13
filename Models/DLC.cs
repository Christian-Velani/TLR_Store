public class DLC
{
    public int IdComplemento { get; set; }
    public string NomeComplemento { get; set; }
    public byte[] Imagem { get; set; }
    public decimal Preco { get; set; }
    public string Descricao { get; set; }
    public EnumStatus Status { get; set; }
}