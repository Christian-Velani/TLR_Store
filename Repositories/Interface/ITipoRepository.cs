public interface ITipoRepository
{
    List<Tipo> BuscarListaJogo(int idJogo);
    List<Tipo> BuscarLista();
    Tipo Buscar(int idTipo);
    void Cadastrar(Tipo tipo);
}