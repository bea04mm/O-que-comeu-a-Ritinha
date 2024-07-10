import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { postTagAPI } from '../../api/Tagsapi';

function Tagscreate() {
    const [tag, setTag] = useState('');
    const [message, setMessage] = useState('');
    const navigate = useNavigate();

    const handleSubmit = (e) => {
        e.preventDefault();
        postTagAPI(tag)
            .then(response => {
                if (response.status === 409) {
                    return response.json().then(data => {
                        throw new Error(data.message);
                    });
                }
                return response.json();
            })
            .then(data => {
                if (data.id) {
                    setTag('');
                    navigate('/tags');
                } else {
                    setMessage('Erro ao criar a tag.');
                }
            })
            .catch(error => {
                setMessage(error.message || 'Erro ao criar a tag.');
            });
    };

    return (
        <div id="backcolor">
            <h1 className="text-center text-white m-0">Criar Tag</h1>
            <div className="justify-content-center text-center">
                <div className="col">
                    <form onSubmit={handleSubmit}>
                        {message && <p className="text-danger m-2">{message}</p>}
                        <div className="form-group m-4">
                            <input
                                type="text"
                                className="form-control text-center"
                                id="tag"
                                value={tag}
                                onChange={(e) => setTag(e.target.value)}
                                required
                            />
                        </div>
                        <div className="form-group m-4">
                            <button type="submit" className="btn btn-light m-2">Criar</button>
                            <Link to={`/tags`} className="btn btn-info m-2">Voltar à lista!</Link>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}

export default Tagscreate;