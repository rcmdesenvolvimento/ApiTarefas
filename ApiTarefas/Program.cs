using ApiTarefas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
opt.UseInMemoryDatabase("TarefasDB"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.MapGet("/", () => "Olá Mundo!!");

//app.MapGet("frases", async () => await new HttpClient().GetStringAsync("https://ron-swanson-quotes.herokuapp.com/v2/quotes"));

// Retorna o Cep de uma rua.
//app.MapGet("Cep", async () => await new HttpClient().GetStringAsync("https://brasilapi.com.br/api/cep/v2/22710310"));

// Busca todas as tarefas.
app.MapGet("/tarefas", async (AppDbContext db) => await db.Tarefas.ToListAsync());

app.MapGet("/tarefas/{id:int}", async (int id, AppDbContext db) =>
    await db.Tarefas.FindAsync(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound("Tarefa não encontrada."));

app.MapGet("/tarefas/concluidas", async (AppDbContext db) =>
    await db.Tarefas.Where(t => t.isConcluida).ToListAsync());

app.MapPost("/tarefas", async (Tarefa tarefa, AppDbContext db) =>
{
    db.Tarefas.Add(tarefa);
    await db.SaveChangesAsync();
    return Results.Created($"/tarefas/{tarefa.Id}", tarefa);
});

app.MapPut("/tarefas/{id:int}", async (int id, Tarefa tarefa, AppDbContext db) =>
{
    var _tarefa = await db.Tarefas.FindAsync(id);

    if (_tarefa is null) return Results.NotFound("Tarefa não encontrda");

    _tarefa.Nome = tarefa.Nome;
    _tarefa.isConcluida = tarefa.isConcluida;
    
    db.Update(_tarefa);
    await db.SaveChangesAsync();

    return Results.Ok("Tarefa alterada com sucesso.");

});

app.MapDelete("/tarefas/{id:int}", async (int id, AppDbContext db) =>
{
    if( await db.Tarefas.FindAsync(id) is Tarefa tarefa)
    {
        db.Tarefas.Remove(tarefa);
        await db.SaveChangesAsync();
        return Results.Ok($"Tarefa {id}, foi excluída."); 
    }
    return Results.NotFound($"Tarefa {id}, não foi encontrda.");
});

//app.UseHttpsRedirection();


app.Run();


class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> Options) : base(Options)
    { }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}

