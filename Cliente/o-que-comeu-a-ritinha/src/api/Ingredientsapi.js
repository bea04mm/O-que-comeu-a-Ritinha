export function getIngredientsAPI() {
    return fetch("https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Ingredients", {
        method: "GET",
        redirect: "follow"
    });
}

export function getIngredientAPI(id) {
    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Ingredients/${id}`, {
        method: "GET",
        redirect: "follow"
    });
}

export function postIngredientAPI(ingredient) {
    return fetch("https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Ingredients/PostIngredients", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ Ingredient: ingredient }),
        redirect: "follow"
    });
}

export function putIngredientAPI(ingredient) {
    const { id, ingredient: ingredientName } = ingredient;

    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Ingredients/PutIngredients/${id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            Id: id,
            Ingredient: ingredientName
        }),
        redirect: "follow"
    });
}

export function deleteIngredientAPI(id) {
    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Ingredients/DeleteIngredients/${id}`, {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
        redirect: "follow"
    });
}