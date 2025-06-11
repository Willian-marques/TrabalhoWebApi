import React, { useState, useEffect } from 'react';
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

  const API_URL = 'http://localhost:5182/' ;

  / --- Funções para interagir com a API ---
  const buscarTarefas = () => {
    fetch(${API_URL}/api/tarefas)
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
    fetch(${API_URL}/api/tarefas, {
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
    <div className="cartao-tarefas">
  <div className="cartao-corpo">
    <div className="area-botao">
      <button onClick={buscarTarefas} className="botao atualizar">Atualizar Lista</button>
    </div>

    <form onSubmit={adicionarTarefa} className="form-adicionar">
      <div className="campo-grupo">
        <input
          type="text"
          className="campo-entrada"
          value={novoTitulo}
          onChange={(e) => setNovoTitulo(e.target.value)}
          placeholder="Adicionar novo título..."
        />
      </div>
      <div className="campo-grupo">
        <textarea
          className="campo-entrada"
          value={novaDescricao}
          onChange={(e) => setNovaDescricao(e.target.value)}
          placeholder="Adicionar descrição (opcional)..."
          rows={2}
        ></textarea>
      </div>
      <button type="submit" className="botao adicionar">Adicionar Tarefa</button>
    </form>

    <form onSubmit={Busca} className="form-buscar">
      <input
        type="text"
        className="campo-entrada"
        placeholder="Buscar título da tarefa..."
        value={termoBusca}
        onChange={(e) => setTermoBusca(e.target.value)}
      />
      <button type="submit" className="botao buscar">Buscar</button>
    </form>

    {erro && <div className="caixa-erro" role="alert">{erro}</div>}
      </div>
    </div>


  )

}

export default ListarTarefas;