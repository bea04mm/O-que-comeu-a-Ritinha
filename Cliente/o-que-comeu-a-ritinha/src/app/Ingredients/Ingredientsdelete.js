import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { getIngredientAPI, deleteIngredientAPI } from '../../api/Ingredientsapi';

function Ingredientsdelete() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [ingredient, setIngredient] = useState(null);

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
                navigate('/ingredientes'); // Redireciona para a página de ingredientes após a exclusão
            })
            .catch(error => console.error('Erro ao apagar o ingrediente:', error));
    };

    return (
        <div id="backcolor" className='text-center'>
            <h1 className="text-white">Apagar</h1>
            <h3 className="text-white">Tens a certeza que queres apagar este ingrediente?</h3>
            {ingredient ? (
                <h3 className="text-black bg-white m-3 p-2 rounded">{ingredient.ingredient}</h3>
            ) : (
                <p className="text-white">Carregando...</p>
            )}
            <button onClick={handleDelete} className="btn btn-danger m-2">Apagar</button>
            <Link to={`/ingredientes`} className="btn btn-info m-2">Voltar à lista!</Link>
        </div>
    );
}

export default Ingredientsdelete;