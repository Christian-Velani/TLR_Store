public class Carrinho
{
    public List<int> idJogos {get; set;}
    public List<int> idComplementos { get; set; }

    public List<Jogo> Jogos {get; set;}
    public List<DLC> Complementos { get; set; }
    public decimal PreçoTotal {get; set;}

}