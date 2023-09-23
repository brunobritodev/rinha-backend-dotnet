namespace RinhaBackend;

public interface IPessoaRepository
{
    Task Insert(Pessoa pessoa);
    Task<Pessoa> GetById(Guid id);
    Task<int> Total();
    Task<List<Pessoa>> ListByTerm(string s);
    Task<bool> GetByApelido(string apelido);
}