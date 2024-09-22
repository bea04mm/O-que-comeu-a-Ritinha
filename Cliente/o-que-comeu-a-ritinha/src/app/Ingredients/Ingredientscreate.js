import React, { useState } from 'react';
import { postIngredientAPI } from '../../api/Ingredientsapi';
import { useNavigate } from 'react-router-dom';

function Ingredientscreate() {
    const [ingredient, setIngredient] = useState('');
    const [message, setMessage] = useState('');
    const navigate = useNavigate();

    const handleSubmit = (e) => {
        e.preventDefault();
        postIngredientAPI(ingredient)
            .then(response => response.json())
            .then(data => {
                if (data.id) {
                    setIngredient('');
                    navigate('/Ingredients');
                } else {
                    setMessage('Erro ao criar o ingrediente.');
                }
            })
            .catch(error => {
                setMessage(error.message || 'Erro ao criar o ingrediente.');
            });
    };

    return (
        <div id="backcolor">
            <h1 className="text-center text-white m-0">Criar Ingrediente</h1>
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
                            <button type="submit" className="btn btn-light m-2">Criar</button>
                            <button type="button" className="btn btn-info m-2" onClick={() => window.history.back()}>
                                Voltar à Lista!
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}

export default Ingredientscreate;