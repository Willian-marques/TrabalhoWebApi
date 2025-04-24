

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

//Estrutura de dado TAREFA.
  var tarefa = new
  {
    Id = new Random().Next(1, 1000),
    Titulo = "Tarefa " + new Random().Next(1, 100),
    Descricao = "Descrição aleatória da tarefa",
    Concluida = false,
    DataCriacao = DateTime.Now
  };

  
//Criando a rota via POST para criar a tarefa ramdomicamente
app.MapPost("/api/tarefas", () =>
{

  //Estrutura de dado TAREFA.
  var tarefa = new
  {
    Id = new Random().Next(1, 1000),
    Titulo = "Tarefa " + new Random().Next(1, 100),
    Descricao = "Descrição aleatória da tarefa",
    Concluida = false,
    DataCriacao = DateTime.Now
  };

  //Adiciona o objeto TAREFA a lista tarefaS
  tarefas.Add(tarefa);
  return Results.Created($"/api/tarefas/{tarefa.Id}", tarefa);

});

//Listando todas as tarefas
app.MapGet("/api/tarefas", () => tarefas);

//Buscando uma tarefa específica
app.MapGet("/api/tarefas/{id}", (int id) =>
{
  var tarefa = tarefas.FirstOrDefault(t => ((dynamic)t).Id == id);
  return tarefa is not null ? Results.Ok(tarefa) : Results.NotFound();
});

//Atualizar uma tarefa especifica
app.MapPut("/api/tarefas/{id}", (int id) =>
{
  var index = tarefas.FindIndex(t => ((dynamic)t).Id == id);
  if(index >= 0){
    var tarefaAntiga = tarefas[index];
    var tarefaAtualizada = new 
    {
      Id = ((dynamic)tarefaAntiga).Id,
      Titulo = ((dynamic)tarefaAntiga).Titulo,
      Descricao = "tarefa atualizada",
      Concluida = true,
      DataCriacao = ((dynamic)tarefaAntiga).DataCriacao
    };
    
    tarefas[index] = tarefaAtualizada;
    return Results.Ok(tarefaAtualizada);
  }
  
  return Results.NotFound();
});


// Excluir uma tarefa
app.MapDelete("/api/tarefas/{id}", (int id) =>
{
    var tarefaIndex = tarefas.FindIndex(t => ((dynamic)t).Id == id);
    
    if (tarefaIndex >= 0)
    {
        var tarefaRemovida = tarefas[tarefaIndex];
        tarefas.RemoveAt(tarefaIndex);
        return Results.Ok(tarefaRemovida);
    }
    
    return Results.NotFound();
});

app.Run();