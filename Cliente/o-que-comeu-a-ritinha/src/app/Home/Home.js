import { useEffect, useState } from "react";
import { getHomeAPI, URL_IMG } from "../../api/Api";
import { getRecipeAPI } from "../../api/Recipesapi";

function Home() {
    const [recipes, setRecipes] = useState([]);

    useEffect(() => {
        // Fetch receitas da API
        getHomeAPI()
            .then(response => response.json())
            .then(data => {
                // Para cada recipe, buscar os detalhes na RecipeAPI
                const fetchRecipeDetails = data.map(recipe => 
                    getRecipeAPI(recipe.recipeFK)
                        .then(response => response.json())
                        .then(recipeDetails => ({
                            ...recipe,
                            ...recipeDetails // Adiciona os detalhes ao objeto recipe
                        }))
                );
                
                // Aguarda todas as promessas serem resolvidas
                Promise.all(fetchRecipeDetails)
                    .then(detailedRecipes => setRecipes(detailedRecipes))
                    .catch(error => {
                        console.error('Erro ao buscar os detalhes das receitas:', error);
                    });
            })
            .catch(error => {
                console.error('Erro ao buscar as receitas:', error);
            });
    }, []);

    return (
        <div id="backcolor" className='text-center'>
            <h1 className="text-white m-2 p-2">Destaques</h1>
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
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default Home;