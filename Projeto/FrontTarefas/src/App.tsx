import React from 'react';
import Header from './components/Header.tsx';
import ListarTarefas from './pages/tarefas/ListarTarefas.tsx';

function App() {
  return (
    <div className="container mt-5">
      <Header />
      <ListarTarefas />
    </div>
  );
}

export default App;

// Componente principal que organiza a estrutura da página, renderizando o cabeçalho e a lista de tarefas.