using ApiTarefas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
opt.UseInMemoryDatabase("TaredasDB"));

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

app.MapPost("/tarefas", async (Tarefa tarefa, AppDbContext db) =>
{
    db.Tarefas.Add(tarefa);
    await db.SaveChangesAsync();
    return Results.Created($"/tarefas/{tarefa.Id}", tarefa);
});

//app.UseHttpsRedirection();


app.Run();


class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> Options) : base(Options)
    { }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}

