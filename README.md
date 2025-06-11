# **Projeto Lista de Tarefas (Aplicação Completa)**

Este documento contém todas as instruções necessárias para configurar e executar a aplicação "Lista de Tarefas", que é composta por um front-end em React e um backend em .NET.

## **✨ Funcionalidades**

* Criar, ler, atualizar e apagar tarefas.  
* Interface interativa para gerir as tarefas.  
* Validações de dados no backend para garantir a integridade.  

## **🛠️ Tecnologias Utilizadas**

| Componente | Tecnologias |
| :---- | :---- |
| **Back-end** | .NET 8, ASP.NET Core, Entity Framework Core, SQLite,|
| **Front-end** | React, Node.js/npm|

## **🚀 Como Executar a Aplicação Completa**

Para executar a aplicação, tanto o back-end quanto o front-end precisam de estar a ser executados em terminais separados.

### **1\. Instruções para o Back-end (.NET API)**

O back-end é a API que o front-end consome.

#### **Pré-requisitos (Back-end)**

* **.NET SDK:** Certifique-se de que tem o SDK do .NET instalado (versão 8 ou superior). Pode descarregar a partir do [site oficial da Microsoft](https://dotnet.microsoft.com/download).  
* **Entity Framework Core Tools:** Se não as tiver, instale as ferramentas de linha de comando do EF Core com o seguinte comando:  
  dotnet tool install \--global dotnet-ef

#### **Passo a Passo (Back-end)**

1. Navegue até à Pasta do Projeto:  
   Abra um terminal e navegue até à pasta raiz do projeto de back-end (ex: ApiTarefas).  
   cd caminho/para/seu/projeto/ApiTarefas

2. Restaure as Dependências:  
   Este comando baixar todos os pacotes NuGet necessários.  
   dotnet restore

3. Crie a Base de Dados:  
   O comando abaixo irá criar o ficheiro da base de dados SQLite (tarefas.db) se ele não existir.  
   dotnet ef database update

4. Execute o Back-end:  
   Inicie a API com o seguinte comando:  
   dotnet run

   O terminal irá mostrar o endereço em que a API está a ser executada (ex: http://localhost:5182). Anote este endereço e **deixe este terminal aberto**.

### **2\. Instruções para o Front-end (React App)**

O front-end é a interface com a qual o utilizador interage.

#### **Pré-requisitos (Front-end)**

* **Node.js e npm:** Necessário para executar o React. Pode baixar a partir do [site oficial do Node.js](https://nodejs.org/).

#### **Passo a Passo (Front-end)**

1. Instale as Dependências:  
   Abra um novo terminal e navegue até à pasta do projeto de front-end (ex: meu-frontend-tarefas).  
   cd caminho/para/seu/projeto/meu-frontend-tarefas

   Execute o comando para instalar as dependências:  
   npm install

2. Configure a Ligação com a API:  
   Abra o ficheiro src/App.js e encontre a constante API\_URL. Certifique-se de que o valor é exatamente o mesmo endereço do seu back-end.  
   const API\_URL \= 'http://localhost:5182'; 

3. Execute o Front-end:  
   No terminal do front-end, execute:  
   npm start

   Uma nova aba deverá abrir no seu navegador em http://localhost:3000.

## **📄 Detalhes da API**

#### **Estrutura dos Endpoints**

| Método | Rota | Descrição |
| :---- | :---- | :---- |
| POST | /api/tarefas | Criar uma nova tarefa |
| GET | /api/tarefas | Listar todas as tarefas |
| GET | /api/tarefas/{id} | Buscar uma tarefa pelo ID |
| PUT | /api/tarefas/{id} | Atualizar uma tarefa |
| DELETE | /api/tarefas/{id} | Remover uma tarefa |

#### **Exemplo de Objeto Tarefa (JSON)**

{  
  "titulo": "Estudar para a prova",  
  "descricao": "Revisar capítulos 1 a 5",  
  "concluida": false  
}  
