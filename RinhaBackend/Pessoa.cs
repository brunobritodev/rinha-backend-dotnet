using RinhaBackend.Utils;

public class Pessoa
{
    public Guid? Id { get; set; }

    public string Apelido { get; set; }

    public string Nome { get; set; }

    public DateOnly? Nascimento { get; set; }

    public List<string>? Stack { get; set; }

    internal bool Valido()
    {
        var atributosInvalidos = !Nascimento.HasValue
                                 || Nome.IsMissing()
                                 || Nome.Length > 100
                                 || Apelido.IsMissing()
                                 || Apelido.Length > 32;

        if (atributosInvalidos)
            return false;

        if (Stack != null && Stack.Any())
            return Stack.All(a => a.IsPresent() && a.Length < 32);


        return true;
    }
}
