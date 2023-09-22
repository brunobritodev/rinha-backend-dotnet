using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RinhaBackend;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton<IPessoaRepository, PessoaRepository>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/pessoas", async ValueTask<Results<Created, UnprocessableEntity>> ([FromServices] IPessoaRepository pessoaRepository, Pessoa pessoa) =>
{
    if (!pessoa.Valido())
        return TypedResults.UnprocessableEntity();

    pessoa.Id = Guid.NewGuid();

    await pessoaRepository.Insert(pessoa);

    return TypedResults.Created($"/pessoas/{pessoa.Id}");
});

app.MapGet("/pessoas/{id}",
async ValueTask<Results<Ok<Pessoa>, NotFound>> ([FromServices] IPessoaRepository pessoaRepository, Guid id) =>
{
    var pessoa = await pessoaRepository.GetById(id);

    return pessoa is not null ? TypedResults.Ok(pessoa) : TypedResults.NotFound();

});

app.MapGet("/pessoas", async ValueTask<Results<Ok<List<Pessoa>>, BadRequest>> ([FromServices] IPessoaRepository pessoaRepository,string t, CancellationToken cancellationToken) =>
{
    if (string.IsNullOrWhiteSpace(t))
        return TypedResults.BadRequest();

    var pessoas = await pessoaRepository.ListByTerm(t);

    return TypedResults.Ok(pessoas);
});

app.MapGet("/contagem-pessoas", async ([FromServices] IPessoaRepository pessoaRepository) =>
{
    var quantidade = await pessoaRepository.Total();

    return Results.Ok(quantidade);
});


app.Run();
