import React, { useState, useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { getTagAPI, putTagAPI } from '../../api/Tagsapi';

function Tagsedit() {
    const { id } = useParams();
    const [tag, setTag] = useState('');
    const [message, setMessage] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        getTagAPI(id)
            .then(response => response.json())
            .then(data => {
                if (data) {
                    setTag(data.tag);
                } else {
                    setMessage('Tag não encontrada.');
                }
            })
            .catch(error => {
                console.error('Erro ao buscar a tag:', error);
                setMessage('Erro ao buscar a tag.');
            });
    }, [id]);

    const handleSubmit = (e) => {
        e.preventDefault();
        const updatedTag = { id, tag: tag };
        putTagAPI(updatedTag)
            .then(response => {
                if (response.status === 409) {
                    return response.json().then(data => {
                        throw new Error(data.message);
                    });
                }
                if (!response.ok) {
                    throw new Error('Erro ao atualizar a tag.');
                }
            })
            .then(() => {
                navigate('/tags');
            })
            .catch(error => {
                console.error('Erro ao atualizar a tag:', error);
                setMessage(error.message || 'Erro ao atualizar a tag.');
            });
    };

    return (
        <div id="backcolor">
            <h1 className="text-center text-white m-0">Editar Tag</h1>
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
                            <button type="submit" className="btn btn-light m-2">Guardar</button>
                            <Link to={`/tags`} className="btn btn-info m-2">Voltar à lista!</Link>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}

export default Tagsedit;
