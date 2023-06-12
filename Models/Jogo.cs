public class Jogo
{
    public int JogoId { get; set; }
    public byte[] Imagem { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string DataLancamento { get; set; }
    public List<Empresa> Desenvolvedora { get; set; }
    public List<Empresa> Distribuidora { get; set; }
    public List<Genero> Genero { get; set; }
    public List<Tipo> Tipo { get; set; }
    public decimal Preco { get; set; }
    public int Desconto { get; set; }
    public List<DLC> Complemento { get; set; }
    public string Requisito { get; set; }
    public EnumStatus Status { get; set; }
    public int ClassificacaoIndicativa { get; set; }
}