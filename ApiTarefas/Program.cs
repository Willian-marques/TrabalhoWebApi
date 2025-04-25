// Implementação de bibliotecas
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

// Criação do builder da aplicação
var builder = WebApplication.CreateBuilder(args);

// Configuração do Swagger para documentação da API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do DbContext com SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tarefas.db"));

// Construção da aplicação
var app = builder.Build();

// Habilita Swagger apenas em ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Criação do banco de dados 
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Endpoint POST para criar nova tarefa
app.MapPost("/api/tarefas", async (Tarefa tarefa, AppDbContext db) =>
{
    // Validação manual do título
    if (string.IsNullOrWhiteSpace(tarefa.Titulo))
    {
        throw new Exception("O título é obrigatório"); // Agora lança Exception
    }

    if (tarefa.Titulo.Length < 3 || tarefa.Titulo.Length > 100)
    {
        throw new Exception("O título deve ter entre 3 e 100 caracteres");
    }

    if (!string.IsNullOrEmpty(tarefa.Descricao) && tarefa.Descricao.Length > 500)
    {
        throw new Exception("A descrição não pode exceder 500 caracteres");
    }

    // Configuração da nova tarefa
    tarefa.Id = 0;
    tarefa.DataCriacao = DateTime.Now;
    
    // Persistência no banco
    db.Tarefas.Add(tarefa);
    await db.SaveChangesAsync();
    
    return Results.Created($"/api/tarefas/{tarefa.Id}", tarefa);
});

//WWWWWWWW

// Endpoint GET para listar todas as tarefas
app.MapGet("/api/tarefas", async (AppDbContext db) => 
    await db.Tarefas.ToListAsync());

// Endpoint GET para obter uma tarefa específica
app.MapGet("/api/tarefas/{id}", async (int id, AppDbContext db) =>
{
    if (id <= 0)
    {
        return Results.BadRequest("ID deve ser maior que zero");
    }
    
    return await db.Tarefas.FindAsync(id) is Tarefa tarefa 
        ? Results.Ok(tarefa) 
        : Results.NotFound();
});

// Endpoint PUT para atualizar uma tarefa
app.MapPut("/api/tarefas/{id}", async (int id, Tarefa inputTarefa, AppDbContext db) =>
{
    if (id <= 0)
    {
        return Results.BadRequest("ID deve ser maior que zero");
    }

    if (string.IsNullOrWhiteSpace(inputTarefa.Titulo))
    {
        return Results.BadRequest("O título é obrigatório");
    }

    if (inputTarefa.Titulo.Length < 3 || inputTarefa.Titulo.Length > 100)
    {
        return Results.BadRequest("O título deve ter entre 3 e 100 caracteres");
    }

    if (!string.IsNullOrEmpty(inputTarefa.Descricao) && inputTarefa.Descricao.Length > 500)
    {
        return Results.BadRequest("A descrição não pode exceder 500 caracteres");
    }

    var tarefa = await db.Tarefas.FindAsync(id);
    
    if (tarefa is null) return Results.NotFound();
    
    tarefa.Titulo = inputTarefa.Titulo;
    tarefa.Descricao = inputTarefa.Descricao;
    tarefa.Concluida = inputTarefa.Concluida;
    
    await db.SaveChangesAsync();
    return Results.NoContent();
});


// MMMMMMM

// Endpoint DELETE para remover uma tarefa
app.MapDelete("/api/tarefas/{id}", async (int id, AppDbContext db) =>
{
    if (id <= 0)
    {
        return Results.BadRequest("ID deve ser maior que zero");
    }

    if (await db.Tarefas.FindAsync(id) is Tarefa tarefa)
    {
        db.Tarefas.Remove(tarefa);
        await db.SaveChangesAsync();
        return Results.Ok(tarefa);
    }
    
    return Results.NotFound();
});

// Inicia a aplicação
app.Run();

// Modelo de dados para Tarefa
public class Tarefa
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 100 caracteres")]
    public string Titulo { get; set; }
    
    [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres")]
    public string Descricao { get; set; }
    
    public bool Concluida { get; set; }
    
    public DateTime DataCriacao { get; set; }
}

// Contexto do banco de dados
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Tarefa> Tarefas { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tarefa>().ToTable("Tarefas");
        
        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.Property(t => t.Titulo).IsRequired();
            entity.Property(t => t.Titulo).HasMaxLength(100);
            entity.Property(t => t.Descricao).HasMaxLength(500);
        });
    }
}
/
//GGGGGGGGGGGGGGGGGG