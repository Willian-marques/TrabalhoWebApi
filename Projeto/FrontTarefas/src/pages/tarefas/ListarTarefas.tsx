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

}