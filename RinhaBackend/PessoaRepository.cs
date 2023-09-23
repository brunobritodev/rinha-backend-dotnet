using Npgsql;

namespace RinhaBackend;

public class PessoaRepository : IPessoaRepository
{
    private readonly NpgsqlDataSource _connection;

    public PessoaRepository(NpgsqlDataSource connection)
    {
        _connection = connection;
    }

    public Task Insert(Pessoa pessoa)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = """INSERT INTO "Pessoas" ("Id", "Apelido", "Nome", "Nascimento", "Stack", "Busca") VALUES (@Id, @Apelido, @Nome, @Nascimento, @Stack, @Busca)""";
        var busca = $"{pessoa.Nome}|{pessoa.Apelido}";

        if (pessoa.Stack is not null)
            busca += string.Join('|', pessoa.Stack);

        cmd.Parameters.AddWithValue("Id", NpgsqlTypes.NpgsqlDbType.Uuid, pessoa.Id);
        cmd.Parameters.AddWithValue("Apelido", NpgsqlTypes.NpgsqlDbType.Varchar, pessoa.Apelido);
        cmd.Parameters.AddWithValue("Nome", NpgsqlTypes.NpgsqlDbType.Varchar, pessoa.Nome);
        cmd.Parameters.AddWithValue("Nascimento", NpgsqlTypes.NpgsqlDbType.Date, pessoa.Nascimento);
        cmd.Parameters.AddWithValue("Stack", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Varchar, pessoa.Stack == null ? DBNull.Value : pessoa.Stack);
        cmd.Parameters.AddWithValue("Busca", NpgsqlTypes.NpgsqlDbType.Text, busca);

        return cmd.ExecuteNonQueryAsync();

    }

    public async Task<List<Pessoa>> ListByTerm(string term)
    {
        var cmd = _connection.CreateCommand();

        cmd.CommandText = """SELECT "Id", "Apelido", "Nome", "Nascimento", "Stack" FROM "Pessoas" WHERE "Busca" LIKE @Term LIMIT 50;""";
        cmd.Parameters.AddWithValue("Term", $"%{term}%");

        await using var reader = await cmd.ExecuteReaderAsync();

        var resultados = new List<Pessoa>();

        while (reader.Read())
        {
            var pessoa = new Pessoa
            {
                Id = reader.GetGuid(0),
                Apelido = reader.GetString(1),
                Nome = reader.GetString(2),
                Nascimento = reader.GetFieldValue<DateOnly>(3),
                Stack = reader.IsDBNull(4) ? null : reader.GetFieldValue<List<string>?>(4)
            };

            resultados.Add(pessoa);
        }

        return resultados;
    }
    public async Task<Pessoa> GetById(Guid id)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = """"SELECT "Id", "Apelido", "Nome", "Nascimento", "Stack" FROM "Pessoas" WHERE "Id" = @Id"""";
        cmd.Parameters.AddWithValue("Id", id);

        await using var reader = await cmd.ExecuteReaderAsync();

        if (reader.Read())
        {
            var pessoa = new Pessoa
            {
                Id = reader.GetGuid(0),
                Apelido = reader.GetString(1),
                Nome = reader.GetString(2),
                Nascimento = reader.GetFieldValue<DateOnly>(3),
                Stack = reader.IsDBNull(4) ? null : reader.GetFieldValue<List<string>?>(4)
            };

            return pessoa;
        }

        return null;
    }

    public async Task<bool> GetByApelido(string apelido)
    {
        var cmd = _connection.CreateCommand();
        cmd.CommandText = """"SELECT EXISTS(SELECT 1 FROM "Pessoas" WHERE "Apelido" = @Apelido);"""";
        cmd.Parameters.AddWithValue("Apelido", apelido);

        bool exists = false;
        await using var reader = await cmd.ExecuteReaderAsync();

        if (reader.Read())
            exists = reader.GetBoolean(0);

        return exists;
    }

    public async Task<int> Total()
    {
        await using var cmd = _connection.CreateCommand();
        cmd.CommandText = """select count(1) from "Pessoas";""";
        var count = await cmd.ExecuteScalarAsync();

        return Convert.ToInt32(count);
    }

}