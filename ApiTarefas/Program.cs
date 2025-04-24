using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do DbContext com SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tarefas.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware para tratar erros globalmente
app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (ValidationException ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { error = "Ocorreu um erro interno" });
    }
});

// Cria o banco de dados e aplica as migrações (apenas para desenvolvimento)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Rotas da API com validações
app.MapPost("/api/tarefas", async (Tarefa tarefa, AppDbContext db) =>
{
    // Validação manual antes de processar
    if (string.IsNullOrWhiteSpace(tarefa.Titulo))
    {
        throw new ValidationException("O título é obrigatório");
    }

    if (tarefa.Titulo.Length < 3 || tarefa.Titulo.Length > 100)
    {
        throw new ValidationException("O título deve ter entre 3 e 100 caracteres");
    }

    if (!string.IsNullOrEmpty(tarefa.Descricao) && tarefa.Descricao.Length > 500)
    {
        throw new ValidationException("A descrição não pode exceder 500 caracteres");
    }

    tarefa.Id = 0; // Para garantir que será gerado novo ID
    tarefa.DataCriacao = DateTime.Now;
    
    db.Tarefas.Add(tarefa);
    await db.SaveChangesAsync();
    
    return Results.Created($"/api/tarefas/{tarefa.Id}", tarefa);
});

app.MapGet("/api/tarefas", async (AppDbContext db) => 
    await db.Tarefas.ToListAsync());

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

app.MapPut("/api/tarefas/{id}", async (int id, Tarefa inputTarefa, AppDbContext db) =>
{
    if (id <= 0)
    {
        return Results.BadRequest("ID deve ser maior que zero");
    }

    // Validação dos dados de entrada
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

app.Run();

// Classe de modelo Tarefa com anotações de validação
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

// Classe de contexto do banco de dados
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Tarefa> Tarefas { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tarefa>().ToTable("Tarefas");
        
        // Configurações adicionais de validação no banco
        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.Property(t => t.Titulo).IsRequired();
            entity.Property(t => t.Titulo).HasMaxLength(100);
            entity.Property(t => t.Descricao).HasMaxLength(500);
        });
    }
}