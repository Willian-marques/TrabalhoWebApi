import React, { useState} from 'react';
// Importa o nosso modelo a partir do caminho correto
import { Tarefa } from '../../models/Tarefa'; 

function ListarTarefas() {
  // --- Estados do componente ---
  const [tarefas, setTarefas] = useState<Tarefa[]>([]);
  const [novoTitulo, setNovoTitulo] = useState('');
  const [novaDescricao, setNovaDescricao] = useState(''); // Estado para a nova descrição
  const [idEmEdicao, setIdEmEdicao] = useState<number | null>(null);
  const [textoEmEdicao, setTextoEmEdicao] = useState('');
  const [descricaoEmEdicao, setDescricaoEmEdicao] = useState(''); // Estado para a descrição em edição
  const [termoBusca, setTermoBusca] = useState('');
  const [filtroAplicado, setFiltroAplicado] = useState(''); 
  const [erro, setErro] = useState<string | null>(null);

  const API_URL = 'http://localhost:5182';

  // --- Funções para interagir com a API ---
  const buscarTarefas = () => {
    fetch(`${API_URL}/api/tarefas`)
      .then((response) => response.json())
      .then((data: Tarefa[]) => {
        setTarefas(data);
        setErro(null);
        setTermoBusca('');
        setFiltroAplicado('');
      })
      .catch((error) => {
        console.error("Erro ao buscar tarefas:", error);
        setErro("Não foi possível ligar à API. Verifique se está em execução.");
      });
  };

  const adicionarTarefa = (e: React.FormEvent) => {
    e.preventDefault();
    if (!novoTitulo.trim()) {
        setErro("O título não pode estar vazio.");
        return;
    }
    const novaTarefa = { titulo: novoTitulo, descricao: novaDescricao, concluida: false };
    fetch(`${API_URL}/api/tarefas`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(novaTarefa),
    }).then(response => {
      if (response.ok) {
        setErro(null);
        setNovoTitulo('');
        setNovaDescricao(''); // Limpa o campo de descrição
        buscarTarefas();
      } else {
        response.json().then(errorData => {
          const mensagemErro = errorData.errors?.erros?.[0] || 'Ocorreu um erro ao adicionar a tarefa.';
          setErro(mensagemErro);
        });
      }
    });
  };

  const atualizarTarefa = (tarefa: Tarefa) => {
    fetch(`${API_URL}/api/tarefas/${tarefa.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(tarefa),
    }).then(response => {
      if (response.ok) {
        setErro(null);
        buscarTarefas();
      } else {
         response.json().then(errorData => {
          const mensagemErro = errorData.errors?.erros?.[0] || 'Ocorreu um erro ao atualizar a tarefa.';
          setErro(mensagemErro);
        });
      }
    });
  };

  const removerTarefa = (id: number) => {
    fetch(`${API_URL}/api/tarefas/${id}`, {
      method: 'DELETE',
    }).then((res) => res.ok && buscarTarefas());
  };

  // --- Funções de controle da UI ---

  const Busca = (e: React.FormEvent) => {
    e.preventDefault();
    setFiltroAplicado(termoBusca);
  };
  
  const IniciarEdicao = (tarefa: Tarefa) => {
    setErro(null);
    setIdEmEdicao(tarefa.id);
    setTextoEmEdicao(tarefa.titulo);
    setDescricaoEmEdicao(tarefa.descricao || ''); // Define a descrição para edição
  };

  const SalvarEdicao = (tarefa: Tarefa) => {
     if(!textoEmEdicao.trim()){
      setErro("O título não pode estar vazio ao editar.");
      return;
    }
    atualizarTarefa({ ...tarefa, titulo: textoEmEdicao, descricao: descricaoEmEdicao });
    setIdEmEdicao(null);
    setTextoEmEdicao('');
    setDescricaoEmEdicao('');
  };

  const MudarStatus = (tarefa: Tarefa) => {
    atualizarTarefa({ ...tarefa, concluida: !tarefa.concluida });
  };

  return (
    // Usando as classes do Bootstrap: card, shadow-sm, p-4 (espaçamento)
    <div className="card shadow-sm p-4">
      <div className="card-body">
        <div className="text-center mb-4">
          <button onClick={buscarTarefas} className="btn btn-info">Atualizar Lista</button>
        </div>

        {/* Formulário com classes do Bootstrap */}
        <form onSubmit={adicionarTarefa} className="mb-3">
          <div className="input-group mb-2">
            <input
              type="text"
              className="form-control" // Classe para campos de texto
              value={novoTitulo}
              onChange={(e) => setNovoTitulo(e.target.value)}
              placeholder="Adicionar novo título..."
            />
          </div>
          <div className="input-group mb-3">
            <textarea
              className="form-control" // Classe para campos de texto
              value={novaDescricao}
              onChange={(e) => setNovaDescricao(e.target.value)}
              placeholder="Adicionar descrição (opcional)..."
              rows={2}
            ></textarea>
          </div>
          <button type="submit" className="btn btn-primary w-100">Adicionar Tarefa</button>
        </form>

        {/* Formulário de busca com classes do Bootstrap */}
        <form onSubmit={Busca} className="input-group mb-4">
          <input
            type="text"
            className="form-control" // Classe para campos de texto
            placeholder="Buscar título da tarefa..."
            value={termoBusca}
            onChange={(e) => setTermoBusca(e.target.value)}
          />
          <button type="submit" className="btn btn-outline-secondary">Buscar</button>
        </form>

        {/* Alerta de erro com classes do Bootstrap */}
        {erro && <div className="alert alert-danger" role="alert">{erro}</div>}

        {/* Lista de tarefas com classes do Bootstrap */}
        <ul className="list-group list-unstyled">
          {tarefas
            .filter(tarefa => tarefa.titulo.toLowerCase().includes(filtroAplicado.toLowerCase()))
            .sort((a, b) => a.titulo.localeCompare(b.titulo))
            .map((tarefa) => (
              <li key={tarefa.id} className="list-group-item">
                {idEmEdicao === tarefa.id ? (
                  // MODO DE EDIÇÃO
                  <div>
                    <div className="input-group mb-2">
                      <input
                        type="text"
                        className="form-control" // Classe para campos de texto
                        value={textoEmEdicao}
                        onChange={(e) => setTextoEmEdicao(e.target.value)}
                      />
                    </div>
                    <div className="input-group mb-2">
                      <textarea
                        className="form-control" // Classe para campos de texto
                        value={descricaoEmEdicao}
                        onChange={(e) => setDescricaoEmEdicao(e.target.value)}
                        rows={2}
                      ></textarea>
                    </div>
                    <div className="btn-group">
                      <button onClick={() => SalvarEdicao(tarefa)} className="btn btn-success">Guardar</button>
                      <button onClick={() => setIdEmEdicao(null)} className="btn btn-secondary">Cancelar</button>
                    </div>
                  </div>
                ) : (
                  // MODO DE VISUALIZAÇÃO
                  <div className="d-flex w-100 align-items-start">
                    <input
                      type="checkbox"
                      className="form-check-input me-3 mt-1" // Classe para checkbox
                      checked={tarefa.concluida}
                      onChange={() => MudarStatus(tarefa)}
                    />
                    <div className="flex-grow-1">
                      <span className={`fw-bold ${tarefa.concluida ? 'text-decoration-line-through text-muted' : ''}`}>
                        {tarefa.titulo}
                      </span>
                      {tarefa.descricao && (
                        <p className={`mb-0 text-muted small ${tarefa.concluida ? 'text-decoration-line-through' : ''}`}>
                          {tarefa.descricao}
                        </p>
                      )}
                    </div>
                    <div className="btn-group ms-3">
                      <button onClick={() => IniciarEdicao(tarefa)} className="btn btn-warning btn-sm">Editar</button>
                      <button onClick={() => removerTarefa(tarefa.id)} className="btn btn-danger btn-sm">X</button>
                    </div>
                  </div>
                )}
              </li>
            ))}
        </ul>
      </div>
    </div>
  );
}

export default ListarTarefas;
