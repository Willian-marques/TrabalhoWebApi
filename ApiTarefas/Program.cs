var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var tarefas = new List<object>();

//Listando todas as tarefas
app.MapGet("/api/tarefas", () => tarefas);

//Buscando uma tarefa específica
app.MapGet("/api/tarefas/{id}", (int id) =>
{
  var tarefa = tarefas.FirstOrDefault(t => ((dynamic)t).Id == id);
  return tarefa is not null ? Results.Ok(tarefa) : Results.NotFound();
});


//Criando a rota via POST para criar a tarefa ramdomicamente
app.MapPost("/api/tarefas", () =>
{

  //Estrutura de dado TAREFA.
  var tarefa = new
  {
    Id = new Random().Next(1, 1000),
    Titulo = "Tarefa" + new Random().Next(1, 100),
    Descricao = "Descrição aleatória da tarefa",
    Concluida = new Random().Next(0, 2) == 1,
    DataCriacao = DateTime.Now
  };

  //Adiciona o objeto TAREFA a lista tarefaS
  tarefas.Add(tarefa);
  return Results.Created($"/api/tarefas/{tarefa.Id}", tarefa);

});

app.Run();