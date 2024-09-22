export function getRecipesAPI() {
    return fetch("https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Recipes", {
        method: "GET",
        redirect: "follow"
    });
}

export function getRecipeAPI(id) {
    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Recipes/${id}`, {
        method: "GET",
        redirect: "follow"
    });
}

export async function postRecipeAPI(formData) {
    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Recipes/PostRecipes`, {
     method: "POST",
     body: formData
    });
}

export async function putRecipeAPI(id, formData) {
    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Recipes/PutRecipes/${id}`, {
     method: "PUT",
     body: formData
    });
}

export function getIngredientsRecipesAPI() {
    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/IngredientsRecipes`, {
        method: "GET",
        redirect: "follow"
    });
}

export function getTagsRecipesAPI() {
    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/RecipesTags`, {
        method: "GET",
        redirect: "follow"
    });
}

export function deleteRecipeAPI(id) {
    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Recipes/DeleteRecipes/${id}`, {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
        redirect: "follow"
    });
}