# **Projeto Lista de Tarefas (Aplicação Completa)**

Este documento contém todas as instruções necessárias para configurar e executar a aplicação "Lista de Tarefas", que é composta por um front-end em React e um backend em .NET.

## **✨ Funcionalidades**

* Criar, ler, atualizar e apagar tarefas.  
* Interface interativa para gerir as tarefas.  
* Validações de dados no backend para garantir a integridade.  

## **🛠️ Tecnologias Utilizadas**

| Componente | Tecnologias |
| :---- | :---- |
| **Back-end** | .NET 8, ASP.NET Core, Entity Framework Core, SQLite|
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
   Este comando descarrega todos os pacotes NuGet necessários.  
   dotnet restore

3. Crie a Base de Dados:  
   O comando abaixo irá criar o ficheiro da base de dados SQLite (tarefas.db) se ele não existir.  
   dotnet ef database update

4. Execute o Back-end:  
   Inicie a API com o seguinte comando:  
   dotnet run

   O terminal irá mostrar o endereço em que a API está a ser executada (ex: http://localhost:5182). Anote este endereço e **deixe este terminal aberto**.

