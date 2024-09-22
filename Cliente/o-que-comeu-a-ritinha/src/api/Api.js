export const URL_IMG = 'https://o-que-comeu-a-ritinha-server.azurewebsites.net/images';

export async function registerUserAPI(formData) {
    try {
        const response = await fetch('https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Autentication/Register', {
            method: 'POST',
            body: formData,
            headers: {
                'Accept': 'application/json',
                // 'Content-Type': 'multipart/form-data', // Não defina isso, pois o FormData cuida disso
            },
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData);
        }

        return await response.json(); // Retorna os dados da resposta, incluindo o UserId
    } catch (error) {
        console.error('Error registering user:', error);
        throw error; // Relança o erro para que possa ser tratado no componente
    }
}

export function getFavoritesAPI() {
    return fetch("https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Favorites", {
        method: "GET",
        redirect: "follow"
    });
}

export function putFavoritesAPI() {
    return fetch("https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/PutFavorites", {
        method: "PUT",
        redirect: "follow"
    });
}

export function getAboutusAPI(id) {
    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Aboutus/${id}`, {
        method: "GET",
        redirect: "follow"
    })
}

export function putAboutusAPI(formData) {
    return fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Aboutus/PutAboutus/1`, {
        method: "PUT",
        body: formData
    });
}

export function getHomeAPI() {
    return fetch("https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/AboutusRecipes", {
        method: "GET",
        redirect: "follow"
    });
}

export function putHomeAPI() {
    return fetch("https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/PutAboutusRecipes", {
        method: "PUT",
        redirect: "follow"
    });
}