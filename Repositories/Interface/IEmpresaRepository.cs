public interface IEmpresaRepository
{
    List<Empresa> BuscarListaTipo(int jogoId, int tipo);
    Empresa Buscar(int empresaId);
    List<Empresa> BuscarLista();
    void Cadastrar(Empresa empresa);
}