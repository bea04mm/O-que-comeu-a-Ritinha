import React, { useState } from 'react';
import { useParams } from 'react-router-dom';

const ConfirmEmail = () => {
    const { userId } = useParams(); // Get userId from URL
    const [token, setToken] = useState('');
    const [statusMessage, setStatusMessage] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        const formData = new FormData();
        formData.append('UserId', userId);
        formData.append('Token', token);

        try {
            const response = await fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Autentication/ConfirmEmail`, {
                method: 'POST',
                body: formData,
            });

            const data = await response.json();

            if (response.ok) {
                setStatusMessage('E-mail confirmado com sucesso! Podes realizar o login.');
            } else {
                setStatusMessage(data.message || 'Ocorreu um erro ao confirmar o e-mail.');
            }
        } catch (error) {
            setStatusMessage('Erro ao confirmar o e-mail: ' + error.message);
        }
    };

    return (
        <div id="backcolor" style={{ textAlign: "center" }}>
            <h1 className="mb-3 text-white">Confirma o teu Email</h1>
            <div>
                <p className="text-white">
                    {statusMessage.includes("Podes realizar")
                        ? statusMessage.slice(0, -6) // Remove last 6 characters
                        : statusMessage}
                    {statusMessage.includes("Podes realizar") && (
                        <a href="/Login"> login.</a>
                    )}
                </p>
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <label className="text-white" htmlFor="token">Token</label>
                        <input
                            id="token"
                            type="text"
                            className="form-control"
                            value={token}
                            onChange={(e) => setToken(e.target.value)}
                            required
                        />
                        <span className="text-danger"></span>
                    </div>
                    <button type="submit" className="m-3 btn btn-lg btn-primary">Confirmar Email</button>
                </form>
            </div>
        </div>
    );
};

export default ConfirmEmail;