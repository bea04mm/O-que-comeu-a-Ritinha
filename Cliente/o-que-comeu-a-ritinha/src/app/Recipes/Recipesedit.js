import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { getIngredientsRecipesAPI, getRecipeAPI, getTagsRecipesAPI, putRecipeAPI } from '../../api/Recipesapi';
import { URL_IMG } from '../../api/Api';
import CustomCKEditor from '../../Components/CustomCKEditor';
import { getIngredientAPI, getIngredientsAPI } from '../../api/Ingredientsapi';
import { getTagAPI, getTagsAPI } from '../../api/Tagsapi';

function Recipesedit() {
    const { id } = useParams();
    const [recipe, setRecipe] = useState({
        title: '',
        image: '',
        time: '',
        portions: '',
        suggestions: '',
        instagram: '',
        steps: '',
    });
    const [ingredients, setIngredients] = useState([]);
    const [selectedIngredients, setSelectedIngredients] = useState([]);
    const [quantities, setQuantities] = useState([]);
    const [tags, setTags] = useState([]);
    const [selectedTags, setSelectedTags] = useState([]);

    const [currentImage, setCurrentImage] = useState('');
    const [newImage, setNewImage] = useState('');

    const [message, setMessage] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
        // Função para ir procurar os ingredientes
        const fetchIngredients = async () => {
            try {
                const response = await getIngredientsRecipesAPI();
                if (!response.ok) {
                    throw new Error('Failed to fetch ingredients');
                }
                const data = await response.json();

                const filteredIngredients = data.filter(ingredient => ingredient.recipeFK === parseInt(id));

                const ingredientsWithNames = await Promise.all(
                    filteredIngredients.map(async (ingredient) => {
                        const ingredientData = await getIngredientAPI(ingredient.ingredientFK);
                        if (!ingredientData.ok) {
                            throw new Error(`Failed to fetch ingredient ${ingredient.ingredientFK}`);
                        }
                        const ingredientDetails = await ingredientData.json();
                        return {
                            ...ingredient,
                            ingredientName: ingredientDetails.ingredient,
                            quantity: ingredient.quantity
                        };
                    })
                );

                setSelectedIngredients(ingredientsWithNames);
                setQuantities(ingredientsWithNames.map(ingredient => ingredient.quantity || ''));
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
                console.log(tagsWithNames)
                setSelectedTags(tagsWithNames); // Atualiza o estado de tags com os dados filtrados e nomes
            } catch (error) {
                console.error('Erro ao buscar as tags:', error);
            }
        };

        fetchTags(); // Chama a função de procura ao montar o componente    

        const fetchRecipe = async () => {
            try {
                const response = await getRecipeAPI(id);
                if (response.ok) {
                    const data = await response.json();
                    setRecipe(data);
                    setCurrentImage(data.image);

                } else {
                    console.error('Erro ao buscar a receita:', response.statusText);
                }
            } catch (error) {
                console.error('Erro ao buscar a receita:', error);
            }
        };

        // Fetch ingredients
        getIngredientsAPI()
            .then(response => response.json())
            .then(data => setIngredients(data))
            .catch(error => console.error('Error fetching ingredients:', error));

        // Fetch tags
        getTagsAPI()
            .then(response => response.json())
            .then(data => setTags(data))
            .catch(error => console.error('Error fetching tags:', error));

        fetchRecipe();
    }, [id]);


    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setRecipe({ ...recipe, [name]: value });
    };

    const handleEditorStepsChange = (data) => {
        setRecipe((prev) => ({
            ...prev,
            steps: data // Atualiza steps
        }));
    };

    const handleEditorSuggestionsChange = (data) => {
        setRecipe((prev) => ({
            ...prev,
            suggestions: data // Atualiza suggestions
        }));
    };

    const handleImageChange = (e) => {
        setNewImage(e.target.files[0]);
    };

    const handleRemoveImage = () => {
        setCurrentImage(''); // Atualiza currentImage para vazio
        setRecipe({ ...recipe, image: '' }); // Remove a imagem da receita
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const formData = new FormData();
        formData.append('Id', recipe.id);
        formData.append('Title', recipe.title);
        formData.append('Time', recipe.time);
        formData.append('Portions', recipe.portions);
        formData.append('Suggestions', recipe.suggestions);
        formData.append('Instagram', recipe.instagram || '');
        formData.append('Steps', recipe.steps);
        // Enviar o nome da imagem atual
        formData.append('CurrentImageName', currentImage);

        // Enviar uma nova imagem apenas se ela for selecionada
        if (newImage) {
            formData.append('ImageRecipe', newImage);
        }


        selectedIngredients.forEach((ingredient, index) => {
            formData.append(`Ingredients[${index}]`, ingredient.ingredientFK); // Acessa o ID do ingrediente
            formData.append(`Quantities[${index}]`, quantities[index]); // Adiciona a quantidade
        });

        selectedTags.forEach((tag, index) => {
            formData.append(`Tags[${index}]`, tag.tagFK); // Acessa o ID da tag
        });

        // Log dos valores para depuração
        for (const value of formData.values()) {
            console.log(value);
        }

        // Chamada à API para atualizar a receita
        try {
            const response = await putRecipeAPI(recipe.id, formData);
            if (response.ok) {
                navigate('/Recipes');
                console.log('Receita atualizada com sucesso!');
            } else {
                setMessage('Erro ao atualizar a receita:', response.statusText);
            }
        } catch (error) {
            setMessage(error.message || 'Erro ao atualizar a receita:', error);
        }
    };

    // Add ingredient to selected list
    const addIngredient = (e) => {
        const ingredientValue = e.target.value;
        const ingredientDetails = ingredients.find(ingredient => ingredient.id === parseInt(ingredientValue)); // Obter os detalhes do ingrediente

        if (ingredientDetails && !selectedIngredients.some(ingredient => ingredient.ingredientFK === ingredientDetails.id)) {
            setSelectedIngredients([...selectedIngredients, { ingredientFK: ingredientDetails.id, ingredientName: ingredientDetails.ingredient }]);
            setQuantities([...quantities, ""]); // Adicionar uma nova quantidade inicializada como string vazia
        }

        e.target.value = ''; // Limpa o valor do seletor
    };

    const handleQuantityChange = (e, index) => {
        const newQuantities = [...quantities];
        newQuantities[index] = e.target.value;
        setQuantities(newQuantities);
    };

    const removeIngredient = (index) => {
        const newSelectedIngredients = [...selectedIngredients];
        const newQuantities = [...quantities];
        newSelectedIngredients.splice(index, 1);
        newQuantities.splice(index, 1);
        setSelectedIngredients(newSelectedIngredients);
        setQuantities(newQuantities);
    };

    // Add tag to selected list
    const addTag = (e) => {
        const tagValue = e.target.value;
        const tagDetails = tags.find(tag => tag.id === parseInt(tagValue)); // Encontre os detalhes da tag
        if (tagDetails && !selectedTags.some(tag => tag.tagFK === tagDetails.id)) {
            setSelectedTags([...selectedTags, { tagFK: tagDetails.id, tagName: tagDetails.tag }]);
        }
    };

    const removeTag = (index) => {
        const newSelectedTags = [...selectedTags];
        newSelectedTags.splice(index, 1); // Remove the tag at the given index
        setSelectedTags(newSelectedTags);  // Update the state with the modified list
    };

    return (
        <div id="backcolor">
            <h1 className="text-center text-white m-0">Editar Receita</h1>
            <div className="justify-content-center">
                <div style={{ textAlign: "center" }} className="row">
                    <div className="col">
                        <form onSubmit={handleSubmit} encType="multipart/form-data">
                        {message && <p className="text-danger m-2">{message}</p>}
                            <div className="form-group m-3">
                                <label className="control-label text-white">Título</label>
                                <input
                                    name="title"
                                    value={recipe.title}
                                    onChange={handleInputChange}
                                    className="form-control text-center mb-2"
                                />
                            </div>

                            <div className="form-group m-3">
                                <label className="control-label text-white">Imagem</label>
                                {currentImage ? (
                                    <div id="imageCurrent">
                                        <img
                                            id="currentImage"
                                            src={`${URL_IMG}/${currentImage}`}
                                            alt="Imagem Atual"
                                            className="rounded"
                                        />
                                        <input type="hidden" name="currentImage" value={currentImage} />
                                        <br />
                                        <button
                                            id="removeImageButton"
                                            type="button"
                                            className="btn btn-sm btn-danger mt-2"
                                            onClick={handleRemoveImage}
                                        >
                                            Remover Imagem
                                        </button>
                                    </div>
                                ) : (
                                    <input type="file" name="imageRecipe" className="form-control mb-2"
                                        accept=".png, .jpg, .jpeg" onChange={handleImageChange} />
                                )}
                            </div>

                            <div className="row">
                                <div className="col-md-6">
                                    <div className="form-group m-3">
                                        <label className="control-label text-white">Tempo</label>
                                        <input
                                            name="time"
                                            type="time"
                                            value={recipe.time}
                                            onChange={handleInputChange}
                                            className="form-control text-center mb-2"
                                        />
                                    </div>
                                </div>
                                <div className="col-md-6">
                                    <div className="form-group m-3">
                                        <label className="control-label text-white">Porções</label>
                                        <input
                                            name="portions"
                                            type="number"
                                            value={recipe.portions}
                                            onChange={handleInputChange}
                                            className="form-control text-center mb-2"
                                        />
                                    </div>
                                </div>
                            </div>

                            <div className="form-group m-3">
                                <label className="control-label text-white">Sugestões</label>
                                <CustomCKEditor name="suggestions" data={recipe.suggestions} onChange={handleEditorSuggestionsChange} />
                            </div>

                            <div className="form-group m-3">
                                <label className="control-label text-white">Instagram</label>
                                <input
                                    name="instagram"
                                    value={recipe.instagram}
                                    onChange={handleInputChange}
                                    className="form-control text-center mb-2"
                                />
                            </div>

                            <div className="form-group m-3">
                                <label className="control-label text-white">Passos</label>
                                <CustomCKEditor name="steps" data={recipe.steps} onChange={handleEditorStepsChange} />
                            </div>

                            {/* Ingredients Selection */}
                            <div className="row m-3">
                                <div className="col-md-3">
                                    <label className="text-white" htmlFor="ingredientSelect">Ingredientes</label>
                                    <select className="form-select" id="ingredientSelect" onChange={addIngredient} value="">
                                        <option disabled value="">Escolha um ingrediente</option>
                                        {ingredients.map((ingredient) => (
                                            <option key={ingredient.id} value={ingredient.id}>
                                                {ingredient.ingredient}
                                            </option>
                                        ))}
                                    </select>
                                </div>
                                <div className="col-md-9">
                                    <label className="text-white">Ingredientes Selecionados</label>
                                    <div id="ListIngredientsSelect">
                                        {selectedIngredients.map((ingredient, index) => (
                                            <div className="row mb-2" key={index}>
                                                <div className="col-4">
                                                    <input
                                                        className="form-control text-center"
                                                        name="Quantities"
                                                        type="text"
                                                        value={quantities[index] || ''}
                                                        onChange={(e) => handleQuantityChange(e, index)}
                                                        placeholder="Quantidade"
                                                    />
                                                </div>
                                                <div className="col-5">
                                                    <input
                                                        type="hidden"
                                                        name="Ingredients"
                                                        value={ingredient.ingredientFK}
                                                    />
                                                    <input
                                                        className="form-control text-center"
                                                        type="text"
                                                        value={ingredient.ingredientName}
                                                        readOnly
                                                    />
                                                </div>
                                                <div className="col-3">
                                                    <button
                                                        className="form-control"
                                                        type="button"
                                                        onClick={() => removeIngredient(index)}
                                                    >
                                                        ❌
                                                    </button>
                                                </div>
                                            </div>
                                        ))}
                                    </div>
                                </div>
                            </div>

                            {/* Tags Selection */}
                            <div className="row m-3">
                                <div className="col-md-3">
                                    <label className="text-white" htmlFor="tagSelect">Tags</label>
                                    <select className="form-select" id="tagSelect" onChange={addTag} value="">
                                        <option disabled value="">Escolha uma tag</option>
                                        {tags.map((tag) => (
                                            <option key={tag.id} value={tag.id}>
                                                {tag.tag}
                                            </option>
                                        ))}
                                    </select>
                                </div>
                                <div className="col-md-9">
                                    <label className="text-white">Tags Selecionadas</label>
                                    <div id="ListTagsSelect">
                                        {selectedTags.map((tag, index) => (
                                            <div className="row mb-2" key={index}>
                                                <div className="col-9">
                                                    <input
                                                        type="hidden"
                                                        name="Tags"
                                                        value={tag.tagFK}
                                                    />
                                                    <input
                                                        className="form-control text-center"
                                                        type="text"
                                                        value={tag.tagName}
                                                        readOnly
                                                    />
                                                </div>
                                                <div className="col-3">
                                                    <button
                                                        className="form-control"
                                                        type="button"
                                                        onClick={() => removeTag(index)}
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
                                    Voltar à Lista!
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Recipesedit;