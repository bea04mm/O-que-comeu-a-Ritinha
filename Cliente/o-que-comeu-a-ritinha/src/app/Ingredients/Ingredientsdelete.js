import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getIngredientAPI, deleteIngredientAPI } from '../../api/Ingredientsapi';

function Ingredientsdelete() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [ingredient, setIngredient] = useState('');

    useEffect(() => {
        getIngredientAPI(id)
            .then(response => response.json())
            .then(data => setIngredient(data))
            .catch(error => console.error('Erro ao buscar o ingrediente:', error));
    }, [id]);

    const handleDelete = () => {
        deleteIngredientAPI(id)
            .then(response => response.text())
            .then(result => {
                console.log(result);
                navigate('/Ingredients'); // Redireciona para a página de ingredientes após a exclusão
            })
            .catch(error => console.error('Erro ao apagar o ingrediente:', error));
    };

    return (
        <div id="backcolor" className='text-center'>
            <h1 className="text-white">Apagar</h1>
            <h3 className="text-white">Tens a certeza que queres apagar este ingrediente?</h3>
            <h3 className="text-black bg-white m-3 p-2 rounded">{ingredient.ingredient}</h3>
            <button onClick={handleDelete} className="btn btn-danger m-2">Apagar</button>
            <button type="button" className="btn btn-info m-2" onClick={() => window.history.back()}>
                Voltar à Lista!
            </button>
        </div>
    );
}

export default Ingredientsdelete;