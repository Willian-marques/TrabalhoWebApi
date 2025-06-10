import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.tsx';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);

// Ponto de entrada da aplicação, responsável por renderizar o componente App principal no HTML.