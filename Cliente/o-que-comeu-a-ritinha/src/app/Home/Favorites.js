import React, { useEffect, useState } from 'react';
import { URL_IMG } from '../../api/Api';
import { getRecipeAPI } from '../../api/Recipesapi';

const getUserFK = async (userId) => {
  const response = await fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Utilizadores`);
  if (!response.ok) throw new Error('Error fetching users');

  const users = await response.json();
  const user = users.find(u => u.userId === userId);
  if (!user) throw new Error('User not found');
  return user.id; // Return the id as userFK
};

const getFavorites = async (userFK) => {
  const response = await fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Favorites`);
  if (!response.ok) throw new Error('Error fetching favorites');
  return await response.json();
};

const Favorites = () => {
  const [error, setError] = useState(null);
  const [recipeDetails, setRecipeDetails] = useState([]); // To store recipe details

  useEffect(() => {
    const fetchData = async () => {
      try {
        const userId = localStorage.getItem('userId');
        const fetchedUserFK = await getUserFK(userId); // Fetch userFK based on userId
        const favoritesData = await getFavorites(fetchedUserFK);

        // Fetch recipe details for each favorite that belongs to the user
        const fetchRecipes = favoritesData.map(async (favorite) => {
          if (favorite.utilizadorFK === fetchedUserFK) {
            const recipeResponse = await getRecipeAPI(favorite.recipeFK);
            const recipe = await recipeResponse.json();
            return {
              ...favorite,
              ...recipe
            };
          }
          return null; // Return null for favorites that don't match
        });

        const detailedRecipes = await Promise.all(fetchRecipes);
        setRecipeDetails(detailedRecipes.filter(Boolean)); // Filter out null values
      } catch (err) {
        setError(err.message);
      }
    };

    fetchData();
  }, []);

  if (error) return <div>Error: {error}</div>;

  return (
    <div id="backcolor" className='text-center'>
      <h1 className="text-white m-2 p-2">Favoritos</h1>
      <div className="row justify-content-center">
        {recipeDetails.map((recipe) => (
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

export default Favorites;