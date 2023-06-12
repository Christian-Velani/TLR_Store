public interface IGeneroRepository
{
    List<Genero> BuscarListaPorJogo(int idJogo);
    List<Genero> BuscarListaCompleta();
    Genero Buscar(int idGenero);
    void Cadastrar(Genero genero);
}