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

  const API_URL = 'http://localhost:5182/'};