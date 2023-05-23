public interface IDLCRepository
{
    List<DLC> BuscarListaDLCJogo(int idJogo);
    List<DLC> BuscarListaDLC();
    DLC Buscar(int idDLC);
    void Cadastrar(DLC Complemento, int idJogo);
    void Atualizar(int idDLC, DLC Complemento);
    void Desativar(int idDLC);
}