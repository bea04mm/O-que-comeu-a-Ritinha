import React, { useEffect, useState } from 'react';
import { getIngredientAPI } from '../../api/Ingredientsapi';
import { Link, useParams } from 'react-router-dom';

function Ingredientsdetails() {
    const { id } = useParams();
    const [ingredient, setIngredient] = useState(null);

    useEffect(() => {
        getIngredientAPI(id)
            .then(response => response.json())
            .then(data => {
                setIngredient(data);
            })
            .catch(error => {
                console.error('Erro ao buscar o ingrediente:', error);
            });
    }, [id]);

    if (!ingredient) {
        return <div>Carregando...</div>;
    }

    return (
        <div id="backcolor" className='text-center'>

            <h1 class="text-white">Detalhes</h1>

            <h3 class="text-black bg-white m-3 p-2 rounded">{ingredient.ingredient}</h3>

            <div class="form-group m-0">
                <Link to={`/ingredientes/editar/${ingredient.id}`} className="btn btn-light m-2">Editar</Link>
                <Link to={`/ingredientes`} className="btn btn-info m-2">Voltar à lista!</Link>
            </div>
        </div>
    );
}

export default Ingredientsdetails;
