import React, { useContext, useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { URL_IMG } from '../../api/Api';
import { getRecipeAPI, getIngredientsRecipesAPI, getTagsRecipesAPI } from '../../api/Recipesapi';
import { getIngredientAPI } from '../../api/Ingredientsapi';
import { getTagAPI } from '../../api/Tagsapi';
import { AuthContext } from '../../Authenticacion/AuthContext';

function Recipesdetails() {
    const { id } = useParams();
    const [recipe, setRecipe] = useState([]);
    const [ingredients, setIngredients] = useState([]);
    const [tags, setTags] = useState([]);
    const [isFav, setIsFav] = useState(false); // Initialize with false or fetch the actual favorite status

    const { roles } = useContext(AuthContext) || {};

    useEffect(() => {
        // Função para ir procurar os ingredientes
        const fetchIngredients = async () => {
            try {
                const response = await getIngredientsRecipesAPI();
                if (!response.ok) {
                    throw new Error('Failed to fetch ingredients');
                }
                const data = await response.json(); // Converte a resposta para JSON

                // Filtrar os ingredientes que têm recipeFK igual ao id da receita atual
                const filteredIngredients = data.filter(ingredient => ingredient.recipeFK === parseInt(id));

                // Procura o nome do ingrediente para cada item filtrado
                const ingredientsWithNames = await Promise.all(
                    filteredIngredients.map(async (ingredient) => {
                        const ingredientData = await getIngredientAPI(ingredient.ingredientFK);
                        if (!ingredientData.ok) {
                            throw new Error(`Failed to fetch ingredient ${ingredient.ingredientFK}`);
                        }
                        const ingredientDetails = await ingredientData.json();
                        return {
                            ...ingredient,
                            ingredientName: ingredientDetails.ingredient // Adiciona o nome do ingrediente ao objeto
                        };
                    })
                );

                console.log(ingredientsWithNames);

                setIngredients(ingredientsWithNames); // Atualiza o estado de ingredientes com os dados filtrados e nomes
            } catch (error) {
                console.error('Erro ao buscar os ingredientes:', error);
            }
        };

        fetchIngredients(); // Chama a função de procura ao montar o componente

        // Função para procurar as tags
        const fetchTags = async () => {
            try {
                const response = await getTagsRecipesAPI();
                if (!response.ok) {
                    throw new Error('Failed to fetch tags');
                }
                const data = await response.json(); // Converte a resposta para JSON

                // Filtrar as tags que têm recipeFK igual ao id da receita atual
                const filteredTags = data.filter(tag => tag.recipeFK === parseInt(id));

                // Procura o nome da tag para cada item filtrado
                const tagsWithNames = await Promise.all(
                    filteredTags.map(async (tag) => {
                        const tagData = await getTagAPI(tag.tagFK);
                        if (!tagData.ok) {
                            throw new Error(`Failed to fetch tag ${tag.tagFK}`);
                        }
                        const tagDetails = await tagData.json();
                        return {
                            ...tag,
                            tagName: tagDetails.tag // Adiciona o nome da tag ao objeto
                        };
                    })
                );

                console.log(tagsWithNames);

                setTags(tagsWithNames); // Atualiza o estado de tags com os dados filtrados e nomes
            } catch (error) {
                console.error('Erro ao buscar as tags:', error);
            }
        };

        fetchTags(); // Chama a função de procura ao montar o componente
   
        getRecipeAPI(id)
            .then(response => response.json())
            .then(data => {
                setRecipe(data);
                // Check if this recipe is a favorite (implement your logic to check)
                checkIfFavorite(data.id); // Call a function to check if the recipe is in favorites
            })
            .catch(error => {
                console.error('Erro ao buscar a receita:', error);
            });
    }, [id]);

    const checkIfFavorite = async (recipeId) => {
        const userId = localStorage.getItem('userId');
        if (!userId) return;

        try {
            const response = await fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Favorites/CheckIfFavorite?recipeId=${recipeId}`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${userId}`,
                },
            });
            if (response.ok) {
                const data = await response.json();
                setIsFav(data.isFavorite); // Assuming the response returns an object with isFavorite
            }
        } catch (error) {
            console.error('Error checking favorite status:', error);
        }
    };

    const addToFavorites = async () => {
        const userId = localStorage.getItem('userId');
        if (!userId) {
            console.error('User is not logged in');
            return;
        }

        try {
            const response = await fetch('https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Favorites/AddToFavorites', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${userId}`,
                },
                body: JSON.stringify(id), // Pass the recipe ID
            });

            if (!response.ok) {
                throw new Error('Failed to add/remove favorite');
            }

            // Toggle the favorite state
            setIsFav(prev => !prev);
            console.log('Favorite updated successfully!');
        } catch (error) {
            console.error('Error:', error);
        }
    };

    if (!recipe || !recipe.image) {
        return <div id="backcolor" className='text-center text-white'>A Carregar...</div>; // Handle loading state or error state here
    }

    return (
        <div id="backcolor" className='text-center'>
            <div className="justify-content-center bg-white m-2 rounded">
                <h1 className="text-black m-0 p-4">{recipe.title}</h1>
                {/* Favoritos */}
                <button onClick={addToFavorites} className="btn btn-link p-0 m-0" style={{ color: '#f0aec0' }}>
                    {isFav ? (
                        <span className="fa-solid fa-star h1"></span> // Filled star for favorite
                    ) : (
                        <span className="fa-regular fa-star h1"></span> // Outline star for non-favorite
                    )}
                </button>
                <div className="d-flex justify-content-center">
                    {tags.map((tag) => (
                        <p key={tag.tagFK} className="border border-dark rounded-pill m-1 p-2"><strong>{tag.tagName}</strong></p>
                    ))}
                </div>
                <div className="d-flex justify-content-center m-0">
                    <h3 className="m-2"><i className="fa-solid fa-clock"></i> {recipe.time}</h3>
                    <h3 className="m-2"><i className="fa-solid fa-person"></i> {recipe.portions}</h3>
                </div>

                <div className="d-flex justify-content-center flex-column flex-lg-row m-2"
                >
                    <img src={`${URL_IMG}/${recipe.image}`} alt={`Receita ${recipe.title}`} title={recipe.title} className="img-fluid rounded m-4" />

                    {recipe.instagram && (
                        <iframe title={recipe.title} className="w-20 h-auto m-4" src={recipe.instagram} frameBorder="0"></iframe>
                    )}
                </div>

                <div className="h3" dangerouslySetInnerHTML={{ __html: recipe.suggestions }} />

                <div className="d-flex justify-content-center flex-column flex-lg-row">
                    <aside className="m-4">
                        <h2 className="p-2">Ingredientes</h2>
                        {ingredients.map((ingredient) => (
                            <div key={ingredient.ingredientFK}>
                                <div className="d-flex text-start justify-content-between gap-4 m-1">
                                    <div>{ingredient.ingredientName}</div>
                                    <div className="text-end">{ingredient.quantity}</div>
                                </div>
                                <hr />
                            </div>
                        ))}
                    </aside>

                    <div className="m-4">
                        <h2 className="p-2">Passos</h2>
                        <div className="text-start" dangerouslySetInnerHTML={{ __html: recipe.steps }} />
                    </div>
                </div>
            </div>
            <div className="form-group m-0">
            {roles.includes('Admin') && (
                <Link to={`/Recipes/Edit/${recipe.id}`} className="btn btn-light m-2">Editar</Link>)}
                <button type="button" className="btn btn-info m-2" onClick={() => window.history.back()}>
                    Voltar à Lista!
                </button>
            </div>
        </div>
    );
};

export default Recipesdetails;