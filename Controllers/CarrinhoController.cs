using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

public class CarrinhoController : Controller
{
    private readonly IJogoRepository _jogoRepository;
    private readonly IDLCRepository _dlcRepository;

    public CarrinhoController(IJogoRepository jogoRepository, IDLCRepository dLCRepository)
    {
        _jogoRepository = jogoRepository;
        _dlcRepository = dLCRepository;
    }

    [HttpGet]
    public ActionResult AdicionarCarrinhoJogo (int idProduto)
    {
        string? session = HttpContext.Session.GetString("carrinho");
        Carrinho? carrinho = JsonSerializer.Deserialize<Carrinho>(session);

        if(carrinho.idJogos == null)
        {
            carrinho.idJogos = new List<int>();
            carrinho.idJogos.Add(idProduto);
            carrinho.Jogos = new List<Jogo>();
            carrinho.Jogos.Add(_jogoRepository.GetJogo(idProduto));
            if(carrinho.PreçoTotal == null)
            {
                carrinho.PreçoTotal = _jogoRepository.GetJogo(idProduto).Preco;
            } else {
                carrinho.PreçoTotal += _jogoRepository.GetJogo(idProduto).Preco;
            }
        } else {
            if(carrinho.idJogos.Contains(idProduto) == false)
            {
                carrinho.idJogos.Add(idProduto);
                carrinho.Jogos.Add(_jogoRepository.GetJogo(idProduto));
                if(carrinho.PreçoTotal == null)
                {
                    carrinho.PreçoTotal = _jogoRepository.GetJogo(idProduto).Preco;
                } else {
                    carrinho.PreçoTotal += _jogoRepository.GetJogo(idProduto).Preco;
                }
            }
        }

        HttpContext.Session.SetString("carrinho",JsonSerializer.Serialize(carrinho));

        return RedirectToAction("VerCarrinho", "Carrinho");
    }

    [HttpGet]
    public ActionResult AdicionarCarrinhoDLC (int idProduto)
    {
        string? session = HttpContext.Session.GetString("carrinho");
        Carrinho? carrinho = JsonSerializer.Deserialize<Carrinho>(session);

        if(carrinho.idComplementos == null)
        {
            carrinho.idComplementos = new List<int>();
            carrinho.idComplementos.Add(idProduto);
            carrinho.Complementos = new List<DLC>();
            carrinho.Complementos.Add(_dlcRepository.Buscar(idProduto));
            if(carrinho.PreçoTotal == null)
                {
                    carrinho.PreçoTotal = _dlcRepository.Buscar(idProduto).Preco;
                } else {
                    carrinho.PreçoTotal += _dlcRepository.Buscar(idProduto).Preco;
                }
        } else {
            if(carrinho.idComplementos.Contains(idProduto) == false)
            {
                carrinho.idComplementos.Add(idProduto);
                carrinho.Complementos.Add(_dlcRepository.Buscar(idProduto));
                if(carrinho.PreçoTotal == null)
                {
                    carrinho.PreçoTotal = _dlcRepository.Buscar(idProduto).Preco;
                } else {
                    carrinho.PreçoTotal += _dlcRepository.Buscar(idProduto).Preco;
                }
            }
        }

        HttpContext.Session.SetString("carrinho",JsonSerializer.Serialize(carrinho));

        return RedirectToAction("VerCarrinho", "Carrinho");        
    }

    [HttpGet]
    public ActionResult RemoverCarrinhoJogo (int idProduto)
    {
        string? session = HttpContext.Session.GetString("carrinho");
        Carrinho? carrinho = JsonSerializer.Deserialize<Carrinho>(session);

        carrinho.idJogos.Remove(idProduto);

        foreach (Jogo jogo in carrinho.Jogos)
        {
            if(jogo.JogoId == idProduto)
            {
                carrinho.PreçoTotal -= jogo.Preco;
                carrinho.Jogos.Remove(jogo);
                break;
            }
        }

        HttpContext.Session.SetString("carrinho",JsonSerializer.Serialize(carrinho));

        return RedirectToAction("VerCarrinho", "Carrinho");        
    }

    [HttpGet]
    public ActionResult RemoverCarrinhoDLC (int idProduto)
    {
        string? session = HttpContext.Session.GetString("carrinho");
        Carrinho? carrinho = JsonSerializer.Deserialize<Carrinho>(session);

        carrinho.idComplementos.Remove(idProduto);

        foreach (DLC complemento in carrinho.Complementos)
        {
            if(complemento.IdComplemento == idProduto)
            {
                carrinho.PreçoTotal -= complemento.Preco;
                carrinho.Complementos.Remove(complemento);
                break;
            }
        }


        HttpContext.Session.SetString("carrinho",JsonSerializer.Serialize(carrinho));

        return RedirectToAction("VerCarrinho", "Carrinho");        
    }

    [HttpGet]
    public ActionResult VerCarrinho()
    {
        string? session = HttpContext.Session.GetString("carrinho");
        Carrinho? carrinho = JsonSerializer.Deserialize<Carrinho>(session);

        return View(carrinho);
    }    
}