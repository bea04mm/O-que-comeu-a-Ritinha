import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getRecipeAPI, deleteRecipeAPI } from '../../api/Recipesapi';

function Recipesdelete() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [recipe, setRecipe] = useState(null);

    useEffect(() => {
        getRecipeAPI(id)
            .then(response => response.json())
            .then(data => setRecipe(data))
            .catch(error => console.error('Erro ao buscar a receita:', error));
    }, [id]);

    const handleDelete = () => {
        deleteRecipeAPI(id)
            .then(response => response.text())
            .then(result => {
                console.log(result);
                navigate('/Recipes'); // Redireciona para a página de receitas após a exclusão
            })
            .catch(error => console.error('Erro ao apagar a receita:', error));
    };

    return (
        <div id="backcolor" className='text-center'>
            <h1 className="text-white">Apagar</h1>
            <h3 className="text-white">Tens a certeza que queres apagar esta receita?</h3>
            {recipe ? (
                <h3 className="text-black bg-white m-3 p-2 rounded">{recipe.title}</h3>
            ) : (
                <p className="text-white">Carregando...</p>
            )}
            <button onClick={handleDelete} className="btn btn-danger m-2">Apagar</button>
            <button type="button" className="btn btn-info m-2" onClick={() => window.history.back()}>
                Voltar à Lista!
            </button>
        </div>
    );
}

export default Recipesdelete;