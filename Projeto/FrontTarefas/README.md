### **Instruções para o Front-end (React App)**

O front-end é a interface com a qual o utilizador interage.

#### **Pré-requisitos (Front-end)**

* **Node.js e npm:** Necessário para executar o React. Pode descarregar a partir do [site oficial do Node.js](https://nodejs.org/).

#### **Passo a Passo (Front-end)**

1. Instale as Dependências:  
   Abra um novo terminal e navegue até à pasta do projeto de front-end (ex: meu-frontend-tarefas).  
   cd caminho/para/seu/projeto/meu-frontend-tarefas

   Execute o comando para instalar as dependências:  
   npm install

2. Configure a Ligação com a API:  
   Abra o ficheiro src/App.js e encontre a constante API\_URL. Certifique-se de que o valor é exatamente o mesmo endereço do seu back-end.  
   const API\_URL \= 'http://localhost:5182'; 

3. Adicione o Estilo Bootstrap:  
   Navegue até à pasta public e abra o ficheiro index.html. Dentro da tag \<head\>, adicione a seguinte linha:  
   \<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet"\>

4. Execute o Front-end:  
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
