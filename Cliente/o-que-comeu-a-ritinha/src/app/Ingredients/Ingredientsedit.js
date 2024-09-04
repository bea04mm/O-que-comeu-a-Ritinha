import React, { useState, useEffect } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { getIngredientAPI, putIngredientAPI } from '../../api/Ingredientsapi';

function Ingredientsedit() {
    const { id } = useParams();
    const [ingredient, setIngredient] = useState('');
    const [message, setMessage] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        getIngredientAPI(id)
            .then(response => response.json())
            .then(data => {
                if (data) {
                    setIngredient(data.ingredient);
                } else {
                    setMessage('Ingrediente não encontrado.');
                }
            })
            .catch(error => {
                console.error('Erro ao buscar o ingrediente:', error);
                setMessage('Erro ao buscar o ingrediente.');
            });
    }, [id]);

    const handleSubmit = (e) => {
        e.preventDefault();
        const updatedIngredient = { id, ingredient };
        putIngredientAPI(updatedIngredient)
            .then(response => {
                if (response.status === 409) {
                    return response.json().then(data => {
                        throw new Error(data.message);
                    });
                }
                if (!response.ok) {
                    throw new Error('Erro ao atualizar o ingrediente.');
                }
                return response;
            })
            .then(() => {
                navigate('/ingredientes');
            })
            .catch(error => {
                console.error('Erro ao atualizar o ingrediente:', error);
                setMessage(error.message || 'Erro ao atualizar o ingrediente.');
            });
    };

    return (
        <div id="backcolor">
            <h1 className="text-center text-white m-0">Editar Ingrediente</h1>
            <div className="justify-content-center text-center">
                <div className="col">
                    <form onSubmit={handleSubmit}>
                        {message && <p className="text-danger m-2">{message}</p>}
                        <div className="form-group m-4">
                            <input
                                type="text"
                                className="form-control text-center"
                                id="ingredient"
                                value={ingredient}
                                onChange={(e) => setIngredient(e.target.value)}
                                required
                            />
                        </div>
                        <div className="form-group m-4">
                            <button type="submit" className="btn btn-light m-2">Guardar</button>
                            <Link to={`/ingredientes`} className="btn btn-info m-2">Voltar à lista!</Link>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}

export default Ingredientsedit;
