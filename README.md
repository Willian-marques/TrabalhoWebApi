# API de Gerenciamento de Tarefas

Esta é uma API simples para gerenciamento de tarefas (To-Do List), construída com .NET 8, Entity Framework Core e SQLite.

## Funcionalidades

- Criar uma nova tarefa
- Listar todas as tarefas
- Buscar uma tarefa pelo ID
- Atualizar uma tarefa existente
- Deletar uma tarefa
- Validações de dados no backend
- Documentação automática com Swagger

## Tecnologias Utilizadas

- [.NET 8](https://dotnet.microsoft.com/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [SQLite](https://www.sqlite.org/index.html)
- [Swagger / Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

## Requisitos

- [.NET SDK 8.0 ou superior](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- (Opcional) Visual Studio, Rider ou VS Code para desenvolvimento

## Como Executar

1. **Clone o repositório**
   ```bash
   git clone [https://github.com/seu-usuario/seu-repositorio.git](https://github.com/Willian-marques/Trabalho-API-web.git)
   cd [seu-repositorio](https://github.com/Willian-marques/Trabalho-API-web.git)
   ```

2. **Restaure os pacotes e execute a aplicação**
   ```bash
   dotnet restore
   dotnet run
   ```

3. **Acesse o Swagger para testar a API**
   - Acesse `http://localhost:5000/swagger` no navegador.

> ⚡ Nota: O banco de dados SQLite (`tarefas.db`) será criado automaticamente na primeira execução.

## Estrutura dos Endpoints

| Método | Rota                 | Descrição                    |
|--------|----------------------|-------------------------------|
| POST   | `/api/tarefas`         | Criar uma nova tarefa         |
| GET    | `/api/tarefas`         | Listar todas as tarefas       |
| GET    | `/api/tarefas/{id}`    | Buscar uma tarefa pelo ID     |
| PUT    | `/api/tarefas/{id}`    | Atualizar uma tarefa existente |
| DELETE | `/api/tarefas/{id}`    | Remover uma tarefa            |

## Exemplo de Objeto `Tarefa`

```json
{
  "titulo": "Estudar para prova",
  "descricao": "Revisar capítulos 1 a 5",
  "concluida": false
}
```

## Observações

- O campo `DataCriacao` é preenchido automaticamente no momento da criação.
- O campo `Id` é gerado automaticamente pelo banco de dados.
- Validações aplicadas:
  - `Titulo` é obrigatório e deve ter entre 3 e 100 caracteres.
  - `Descricao` é opcional, mas limitada a 500 caracteres.

## Melhorias Futuras (Sugestões)

- Implementar autenticação/autorização
- Adicionar paginação na listagem de tarefas
- Adicionar filtros por status (`concluída` ou `não concluída`)
