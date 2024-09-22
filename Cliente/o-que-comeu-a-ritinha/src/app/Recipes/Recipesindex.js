import React, { useState, useEffect, useContext } from 'react';
import { getRecipesAPI } from '../../api/Recipesapi';
import { URL_IMG } from '../../api/Api';
import { AuthContext } from '../../Authenticacion/AuthContext';

function Recipesindex() {
    const { roles } = useContext(AuthContext) || {};

    const [recipes, setRecipes] = useState([]);

    useEffect(() => {
        // Fetch receitas da API
        getRecipesAPI()
            .then(response => response.json())
            .then(data => {
                setRecipes(data); // Atualiza o estado com os dados recebidos
            })
            .catch(error => {
                console.error('Erro ao buscar as receitas:', error);
            });
    }, []);

    return (
        <div id="backcolor" className='text-center'>
            <h1 className="text-white m-0">Receitas</h1>
            {roles.includes('Admin') && (
            <p className="text-center m-4">
                <a href="Recipes/Create" className="btn btn-light">Criar nova Receita!</a>
            </p>)}

            {/* Search Bar */}
            <form className="m-4 w-50 mx-auto">
                <input
                    type="text"
                    className="form-control text-center"
                    placeholder="Pesquisar..."
                />
            </form>

            <div className="row justify-content-center">
                {recipes.map((recipe) => (
                    <div className="col-md-3 mb-4" key={recipe.id}>
                        <div className="text-center bg-white rounded p-3 h-100 w-auto d-inline-block">
                            <a href={`/Recipes/Details/${recipe.id}`} className="text-black text-decoration-none">
                                <img
                                    src={`${URL_IMG}/${recipe.image}`}
                                    alt={`Receita ${recipe.title}`}
                                    title={recipe.title}
                                    className="rounded img-fluid"
                                />
                                <p className="mt-2 mb-0">{recipe.title}</p>
                            </a>
                            {roles.includes('Admin') && (<>
                            <a href={`/Recipes/Edit/${recipe.id}`} className="btn btn-outline-info btn-sm btn-block m-1">Editar</a>
                            <a href={`/Recipes/Delete/${recipe.id}`} className="btn btn-outline-danger btn-sm btn-block m-1">Apagar</a></>)}
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default Recipesindex;