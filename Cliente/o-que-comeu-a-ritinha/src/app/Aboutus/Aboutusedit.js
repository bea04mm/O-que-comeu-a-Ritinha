import React, { useState, useEffect } from 'react';
import { getAboutusAPI, putAboutusAPI, URL_IMG, getHomeAPI } from '../../api/Api';
import CustomCKEditor from '../../Components/CustomCKEditor';
import { getRecipeAPI, getRecipesAPI } from '../../api/Recipesapi';
import { useNavigate } from 'react-router-dom';

function Aboutusedit() {
    const [aboutUs, setAboutUs] = useState({
        description: '',
        imageDescription: '',
        imageLogo: ''
    });
    const [recipes, setRecipes] = useState([]);
    const [selectedRecipes, setSelectedRecipes] = useState([]);

    const [currentImageDescription, setCurrentImageDescription] = useState('');
    const [newImageDescription, setNewImageDescription] = useState('');
    const [currentImageLogo, setCurrentImageLogo] = useState('');
    const [newImageLogo, setNewImageLogo] = useState('');

    const [message, setMessage] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        // Fetch 'About Us' data
        getAboutusAPI(1)
            .then(response => response.json())
            .then(data => {
                setAboutUs(data);
                setCurrentImageDescription(data.imageDescription);
                setCurrentImageLogo(data.imageLogo);
            })
            .catch(error => console.error('Error fetching About Us data:', error));

        // Fetch recipes
        getRecipesAPI()
            .then(response => response.json())
            .then(data => setRecipes(data))
            .catch(error => console.error('Error fetching recipes:', error));

        // Função para procurar as tags
        const fetchHome = async () => {
            try {
                const response = await getHomeAPI();
                if (!response.ok) {
                    throw new Error('Failed to fetch Home');
                }
                const data = await response.json(); // Converte a resposta para JSON

                // Procura o nome da tag para cada item filtrado
                const recipesWithNames = await Promise.all(
                    data.map(async (home) => {
                        const homeData = await getRecipesAPI(home.recipeFK);
                        if (!homeData.ok) {
                            throw new Error(`Failed to fetch recipe ${home.recipeFK}`);
                        }
                        const homeDetails = await homeData.json();
                        return {
                            ...home,
                            homeName: homeDetails.title // Adiciona o nome da tag ao objeto
                        };
                    })
                );
                console.log(recipesWithNames)
                setSelectedRecipes(recipesWithNames); // Atualiza o estado de tags com os dados filtrados e nomes
            } catch (error) {
                console.error('Erro ao buscar as receitas:', error);
            }
        };

        fetchHome();
        // Fetch additional details for selected recipes
        getHomeAPI()
            .then(response => response.json())
            .then(data => {
                const fetchRecipeDetails = data.map(recipe =>
                    getRecipeAPI(recipe.recipeFK)
                        .then(response => response.json())
                        .then(recipeDetails => ({
                            ...recipe,
                            ...recipeDetails
                        }))
                );
                Promise.all(fetchRecipeDetails)
                    .then(detailedRecipes => setSelectedRecipes(detailedRecipes))
                    .catch(error => console.error('Error fetching recipe details:', error));
            })
            .catch(error => console.error('Error fetching home data:', error));
    }, []);

    const handleEditorChange = (data) => {
        setAboutUs((prev) => ({
            ...prev,
            description: data
        }));
    };

    const handleImageDescriptionChange = (e) => {
        setNewImageDescription(e.target.files[0]);
    };

    const handleImageDescriptionRemove = () => {
        setCurrentImageDescription(''); // Atualiza currentImage para vazio
        setAboutUs({ ...aboutUs, imageDescription: '' }); // Remove a imagem da receita
    };

    const handleImageLogoChange = (e) => {
        setNewImageLogo(e.target.files[0]);
    };

    const handleImageLogoRemove = () => {
        setCurrentImageLogo(''); // Atualiza currentImage para vazio
        setAboutUs({ ...aboutUs, imageLogo: '' }); // Remove a imagem da receita
    };


    const handleRecipeSelect = (e) => {
        const recipeValue = e.target.value;
        const recipeDetails = recipes.find((r) => r.id === parseInt(recipeValue, 10));

        if (!recipeDetails) {
            console.error("Recipe not found!");
            return;
        }

        if (selectedRecipes.length >= 3) {
            alert("Você só pode selecionar até 3 receitas.");
            return;
        }

        if (selectedRecipes.find((r) => r.id === parseInt(recipeValue, 10))) {
            alert(`${recipeDetails.title} já está na lista de receitas.`);
            return;
        }

        if (recipeDetails && !selectedRecipes.some(recipe => recipe.recipeFK === recipeDetails.id)) {
            setSelectedRecipes([...selectedRecipes, { recipeFK: recipeDetails.id, title: recipeDetails.title}]);
        }
    };

    const handleRemoveRecipe = (index) => {
        const newSelectedRecipes = [...selectedRecipes];
        newSelectedRecipes.splice(index, 1); // Remove the tag at the given index
        setSelectedRecipes(newSelectedRecipes);
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        const formData = new FormData();
        formData.append('Id', aboutUs.id);
        formData.append('Description', aboutUs.description);
        formData.append('CurrentImageDescription', currentImageDescription);
        formData.append('CurrentImageLogo', currentImageLogo);

        if (newImageDescription) {
            formData.append('ImageDescription', newImageDescription);
        }

        if (newImageLogo) {
            formData.append('ImageLogo', newImageLogo);
        }

        selectedRecipes.forEach((recipe, index) => {
            formData.append(`Recipes[${index}]`, recipe.recipeFK); // Acessa o ID da tag
        });

        putAboutusAPI(formData)
            .then(response => {
                if (response.ok) {
                    navigate('/AboutUs');
                    return response.json();
                }
                throw new Error('Update failed');
            })
            .then(result => {
                console.log('Update successful:', result);
            })
            .catch(error => {
                setMessage(error.message || 'Erro ao atualizar');
            });
    };

    return (
        <div id="backcolor">
            <h1 className="text-center text-white m-0">Acerca de Nós</h1>
            <div className="justify-content-center">
                <div style={{ textAlign: "center" }} className="row">
                    <div className="col">
                        <form onSubmit={handleSubmit} encType="multipart/form-data">
                        {message && <p className="text-danger m-2">{message}</p>}
                            <div className="form-group m-3">
                                <label className="control-label text-white">Descrição para Acerca de Nós</label>
                                <CustomCKEditor name="description" data={aboutUs.description} onChange={handleEditorChange} />
                            </div>

                            <div className="row">
                                <div className="col-md-6">
                                    <div className="form-group m-3">
                                        <label className="control-label text-white">Imagem para Acerca de Nós</label>
                                        {currentImageDescription ? (
                                            <div id="ImageDescriptionCurrent">
                                                <img
                                                    src={`${URL_IMG}/${currentImageDescription}`}
                                                    alt="Current Description"
                                                    className="rounded"
                                                />
                                                <input type="hidden" name="currentImageDescription" value={currentImageDescription} />
                                                <br />
                                                <button
                                                    id='removeImageButton'
                                                    type="button"
                                                    className="btn btn-sm btn-danger mt-2"
                                                    onClick={handleImageDescriptionRemove}
                                                >
                                                    Remover Imagem
                                                </button>
                                            </div>
                                        ) : (
                                            <input
                                                type="file"
                                                name="imageDescription"
                                                accept=".png, .jpg, .jpeg"
                                                className="form-control mb-2"
                                                onChange={handleImageDescriptionChange}
                                            />
                                        )}
                                    </div>
                                </div>

                                <div className="col-md-6">
                                    <div className="form-group m-3">
                                        <label className="control-label text-white">Imagem para Logo</label>
                                        {currentImageLogo ? (
                                            <div id="ImageLogoCurrent">
                                                <img
                                                    src={`${URL_IMG}/${currentImageLogo}`}
                                                    alt="Current Logo"
                                                    className="rounded"
                                                />
                                                <input type="hidden" name="currentImageLogo" value={currentImageLogo} />
                                                <br />
                                                <button
                                                    id='removeImageButton'
                                                    type="button"
                                                    className="btn btn-sm btn-danger mt-2"
                                                    onClick={handleImageLogoRemove}
                                                >
                                                    Remover Imagem
                                                </button>
                                            </div>
                                        ) : (
                                            <input
                                                type="file"
                                                name="imageLogo"
                                                accept=".png, .jpg, .jpeg"
                                                className="form-control mb-2"
                                                onChange={handleImageLogoChange}
                                            />
                                        )}
                                    </div>
                                </div>
                            </div>

                            <div className="row m-3">
                                <div className="col-md-3">
                                    <label htmlFor="recipeSelect" className="text-white">Receitas</label>
                                    <select
                                        className="form-select"
                                        id="recipeSelect"
                                        onChange={handleRecipeSelect}
                                        value=""
                                    >
                                        <option disabled value="">
                                            Escolha três receitas
                                        </option>
                                        {recipes.map((recipe) => (
                                            <option key={recipe.id} value={recipe.id}>
                                                {recipe.title}
                                            </option>
                                        ))}
                                    </select>
                                </div>
                                <div className="col-md-9">
                                    <label className="text-white">Receitas Selecionadas</label>
                                    <div id="ListRecipesSelect">
                                        {selectedRecipes.map((recipe, index) => (
                                            <div className="row mb-2" key={index}>
                                                <div className="col-9">
                                                    <input
                                                        type="hidden"
                                                        name="listRecipesA"
                                                        value={recipe.recipeFK}
                                                    />
                                                    <input
                                                        className="form-control text-center"
                                                        type="text"
                                                        value={recipe.title}
                                                        readOnly
                                                    />
                                                </div>
                                                <div className="col-3">
                                                    <button
                                                        className="form-control"
                                                        type="button"
                                                        onClick={() => handleRemoveRecipe(index)}
                                                    >
                                                        ❌
                                                    </button>
                                                </div>
                                            </div>
                                        ))}
                                    </div>
                                </div>
                            </div>

                            <div className="form-group m-2">
                                <input type="submit" value="Guardar" className="btn btn-light m-2" />
                                <button type="button" className="btn btn-info m-2" onClick={() => window.history.back()}>
                                    Voltar
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Aboutusedit;