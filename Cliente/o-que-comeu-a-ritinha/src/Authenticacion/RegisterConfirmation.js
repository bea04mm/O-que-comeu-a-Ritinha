import React from 'react';
import { useLocation } from 'react-router-dom';

const RegisterConfirmation = () => {
  const query = new URLSearchParams(useLocation().search);
  const userId = query.get('userId'); // Obtém o userId da URL

  return (
    <div id="backcolor" style={{ textAlign: "center" }}>
      <h1 className="mb-3 text-white">Confirmação de Registo</h1>
      <p className="text-white">
        Um email foi enviado para ti. Por favor, vê a tua caixa de entrada para teres o teu token de confirmação.
      </p>
      <p className="text-white">
        Assim que receberes o teu token, por favor <a href={`/ConfirmEmail/${userId}`} className="text-info">clica aqui</a> para confirmares a tua conta.
      </p>
    </div>
  );
};

export default RegisterConfirmation;