using Microsoft.EntityFrameworkCore;
using TarefasApi.Data;
using TarefasApi.Models;

var builder = WebApplication.CreateBuilder(args);

// CORS 
//Permissão do Back-End com o Front-End

var regraAcessoBack = "regraAcessoBack"; 
//criando as condições de acesso ao back-end
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: regraAcessoBack,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000") //aqui ele permite que o front-end acesse o back-end, 
                                                                      // e tem que vir da "3000"
                                .AllowAnyHeader() // pode exibir qualquer coisa
                                .AllowAnyMethod(); // pode trazer qualquer metodo
                      });
});


//  Configurando o banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tarefas.db"));

var app = builder.Build();



// Aplica as regras do CORS
app.UseCors(regraAcessoBack);

// Endpoint POST para criar nova tarefa
        app.MapPost("/api/tarefas", async (Tarefa tarefa, AppDbContext db) => // aqui vai criar a rota e enviar via post para o banco de dados
        {
            var erros = tarefa.Validar(); // vai inciar a cadeia de verificação, essa função Validar() os possiveis erros setados
            if (erros.Any())
            {
                return Results.ValidationProblem(new Dictionary<string, string[]> { { "erros", erros.ToArray() } });
            }

            //se nao houver erros, vai criar a tarefa
            tarefa.Id = 0; // aqui vai criar o id da tarefa
            tarefa.DataCriacao = DateTime.Now; // aqui vai criar a data de criação da tarefa
            
            db.Tarefas.Add(tarefa); // aqui vai adicionar a tarefa no banco de dados
            await db.SaveChangesAsync(); // aqui vai salvar a tarefa no banco de dados
            
            return Results.Created($"/api/tarefas/{tarefa.Id}", tarefa); // aqui vai criar a tarefa e enviar a resposta para o front-end
        });

        // Endpoint GET para listar todas as tarefas
        app.MapGet("/api/tarefas", (AppDbContext db) => // aqui vai criar a rota e buscar via Get usando o banco de dados
            db.Tarefas.ToListAsync()); // aqui vai buscar todas as tarefas no banco de dados e enviar a resposta para o front

        // Endpoint GET para obter uma tarefa específica
        app.MapGet("/api/tarefas/{id}", async (int id, AppDbContext db) => // Buscar via ID
        {
            if (id <= 0)
            {
                return Results.BadRequest("O ID deve ser maior que zero.");
            }
            return await db.Tarefas.FindAsync(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound();
        });

        // Endpoint PUT para atualizar uma tarefa
        app.MapPut("/api/tarefas/{id}", async (int id, Tarefa inputTarefa, AppDbContext db) => // aqui vai criar a rota e atualizar via Put usando o banco de dados
        {
            if (id <= 0)
            {
                return Results.BadRequest("O ID deve ser maior que zero.");
            }
            
            var tarefa = await db.Tarefas.FindAsync(id); // aqui vai buscar a tarefa no banco de dados
            if (tarefa is null) return Results.NotFound();

            var erros = inputTarefa.Validar();
            if (erros.Any())
            {
                return Results.ValidationProblem(new Dictionary<string, string[]> { { "erros", erros.ToArray() } });
            }

            // caso passe pela validação sem entrar, vai atualizar a tarefa
            tarefa.Titulo = inputTarefa.Titulo;
            tarefa.Descricao = inputTarefa.Descricao;
            tarefa.Concluida = inputTarefa.Concluida;

            //aqui salva a tarefa e salva no banco
            await db.SaveChangesAsync(); 
            return Results.NoContent();
        });

        // Endpoint DELETE para remover uma tarefa
        app.MapDelete("/api/tarefas/{id}", async (int id, AppDbContext db) => // deleta via ID
        {
            if (id <= 0)
            {
                return Results.BadRequest("O ID deve ser maior que zero.");
            }
            
            if (await db.Tarefas.FindAsync(id) is Tarefa tarefa) // aqui vai buscar a tarefa no banco de dados
            {
                db.Tarefas.Remove(tarefa); 
                await db.SaveChangesAsync(); // vai remover a tarefa e salvar no banco
                return Results.Ok(tarefa);
            }
            
            return Results.NotFound();
        });
    


// Inicia a aplicação
app.Run();
