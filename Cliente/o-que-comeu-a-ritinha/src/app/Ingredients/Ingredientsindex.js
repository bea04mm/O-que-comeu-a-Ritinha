import React, { useEffect, useState } from 'react';
import { getIngredientsAPI } from '../../api/Ingredientsapi';
import { Link } from 'react-router-dom';

function Ingredientsindex() {
    const [ingredients, setIngredients] = useState([]);

    useEffect(() => {
        // Fetch ingredientes da API
        getIngredientsAPI()
            .then(response => response.json())
            .then(data => {
                setIngredients(data); // Atualiza o estado com os dados recebidos
            })
            .catch(error => {
                console.error('Erro ao buscar os ingredientes:', error);
            });
    }, []);

    return (
        <div id="backcolor">
            <h1 className="text-center text-white m-0">Ingredientes</h1>
            <p className="text-center m-4">
                <a href="Ingredients/Create" className="btn btn-light">Criar novo Ingrediente!</a>
            </p>
            <div className="justify-content-center">
                <table className="table table-custom text-center m-0">
                    <thead>
                        <tr>
                            <th>Ingrediente</th>
                            <th>Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        {ingredients.map(ingredient => (
                            <tr key={ingredient.id}>
                                <td>{ingredient.ingredient}</td>
                                <td>
                                    <Link to={`/Ingredients/Edit/${ingredient.id}`} className="btn btn-outline-secondary btn-sm btn-block m-1">Editar</Link>
                                    <Link to={`/Ingredients/Delete/${ingredient.id}`} className="btn btn-outline-danger btn-sm btn-block m-1">Apagar</Link>
                                    <Link to={`/Ingredients/Details/${ingredient.id}`} className="btn btn-outline-info btn-sm btn-block m-1">Detalhes</Link>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default Ingredientsindex;