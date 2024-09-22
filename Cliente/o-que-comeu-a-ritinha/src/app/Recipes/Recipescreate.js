import React, { useEffect, useState } from "react";
import { postRecipeAPI } from "../../api/Recipesapi";
import CustomCKEditor from "../../Components/CustomCKEditor";
import { getIngredientsAPI } from "../../api/Ingredientsapi";
import { getTagsAPI } from "../../api/Tagsapi";
import { useNavigate } from "react-router-dom";

function Recipescreate() {
    // State for form fields
    const [title, setTitle] = useState('');
    const [image, setImage] = useState('');
    const [time, setTime] = useState('');
    const [portions, setPortions] = useState('');
    const [suggestions, setSuggestions] = useState('');
    const [instagram, setInstagram] = useState('');
    const [steps, setSteps] = useState('');
    const [ingredients, setIngredients] = useState([]);
    const [selectedIngredients, setSelectedIngredients] = useState([]);
    const [quantities, setQuantities] = useState([]);
    const [tags, setTags] = useState([]);
    const [selectedTags, setSelectedTags] = useState([]);

    const [message, setMessage] = useState('');
    const navigate = useNavigate();

    useEffect(() => {
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
    }, []);

    // Handle form submission
    const handleSubmit = (e) => {
        e.preventDefault();
        const formData = new FormData();
        formData.append("Title", title);
        formData.append("ImageRecipe", image);
        formData.append("Time", time);
        formData.append("Portions", portions);
        formData.append("Suggestions", suggestions);
        formData.append("Instagram", instagram || '');
        formData.append("Steps", steps);

        // Append ingredients and quantities as individual entries
        selectedIngredients.forEach((ingredient, index) => {
            formData.append(`Ingredients[${index}]`, ingredient);
            formData.append(`Quantities[${index}]`, quantities[index]);
        });

        // Append tags as individual entries
        selectedTags.forEach((tag, index) => {
            formData.append(`Tags[${index}]`, tag);
        });

        for (const value of formData.values()) {
            console.log(value);
        }

        try {
            const response = postRecipeAPI(formData);
            if (response.ok) {
                navigate('/Recipes');
                console.log('Receita atualizada com sucesso!');
            } else {
                setMessage('Erro ao criar a receita.');
            }
        } catch (error) {
            setMessage(error.message || 'Erro ao criar a receita.');
        }
    };

    // Handle file input change
    const handleImageChange = (e) => {
        setImage(e.target.files[0]);
    };

    // Add ingredient to selected list
    const addIngredient = (e) => {
        const ingredientValue = e.target.value;

        // Only add the ingredient if it hasn't been added yet
        if (!selectedIngredients.includes(ingredientValue)) {
            setSelectedIngredients([...selectedIngredients, ingredientValue]);
            setQuantities([...quantities, ""]); // Initialize the quantity as an empty string
        }
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
        if (!selectedTags.includes(tagValue)) {
            setSelectedTags([...selectedTags, tagValue]);
        }
    };

    const removeTag = (index) => {
        const newSelectedTags = [...selectedTags];
        newSelectedTags.splice(index, 1); // Remove the tag at the given index
        setSelectedTags(newSelectedTags);  // Update the state with the modified list
    };

    return (
        <div id="backcolor">
            <h1 className="text-center text-white m-0">Criar Receita</h1>
            <div className="justify-content-center">
                <div style={{ textAlign: "center" }} className="row">
                    <div className="col">
                        <form onSubmit={handleSubmit} encType="multipart/form-data">
                        {message && <p className="text-danger m-2">{message}</p>}
                            <div className="form-group m-3">
                                <label className="control-label text-white">Título</label>
                                <input
                                    type="text"
                                    className="form-control text-center mb-2"
                                    value={title}
                                    onChange={(e) => setTitle(e.target.value)}
                                />
                            </div>

                            <div className="form-group m-3">
                                <label className="control-label text-white">Imagem</label>
                                <input
                                    type="file"
                                    name="imageRecipe"
                                    accept=".png, .jpg, .jpeg"
                                    className="form-control mb-2"
                                    onChange={handleImageChange}
                                />
                            </div>

                            <div className="row">
                                <div className="col-md-6">
                                    <div className="form-group m-3">
                                        <label className="control-label text-white">Tempo</label>
                                        <input
                                            type="time"
                                            className="form-control text-center mb-2"
                                            value={time}
                                            onChange={(e) => setTime(e.target.value)}
                                            required
                                        />
                                    </div>
                                </div>
                                <div className="col-md-6">
                                    <div className="form-group m-3">
                                        <label className="control-label text-white">Portions</label>
                                        <input
                                            type="number"
                                            className="form-control text-center mb-2"
                                            value={portions}
                                            onChange={(e) => setPortions(e.target.value)}
                                            required
                                        />
                                    </div>
                                </div>
                            </div>

                            <div className="form-group m-3">
                                <label className="control-label text-white">Sugestões</label>
                                <CustomCKEditor
                                    value={suggestions || ""}
                                    onChange={(data) => setSuggestions(data)}
                                />

                            </div>

                            <div className="form-group m-3">
                                <label className="control-label text-white">Instagram</label>
                                <input
                                    type="text"
                                    className="form-control text-center mb-2"
                                    value={instagram}
                                    onChange={(e) => setInstagram(e.target.value)}
                                />
                            </div>

                            <div className="form-group m-3">
                                <label className="control-label text-white">Passos</label>
                                <CustomCKEditor
                                    value={steps || ""}
                                    onChange={(data) => setSteps(data)}
                                />
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
                                        {selectedIngredients.map((ingredient, index) => {
                                            const ingredientDetails = ingredients.find(i => i.id === parseInt(ingredient));
                                            return (
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
                                                            value={ingredient}
                                                        />
                                                        <input
                                                            className="form-control text-center"
                                                            type="text"
                                                            value={ingredientDetails?.ingredient}
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
                                            );
                                        })}
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
                                        {selectedTags.map((tag, index) => {
                                            const tagDetails = tags.find(t => t.id === parseInt(tag));
                                            return (
                                                <div className="row mb-2" key={index}>
                                                    <div className="col-9">
                                                        <input
                                                            type="hidden"
                                                            name="Tags"
                                                            value={tag}
                                                        />
                                                        <input
                                                            className="form-control text-center"
                                                            type="text"
                                                            value={tagDetails?.tag}
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
                                            );
                                        })}
                                    </div>
                                </div>
                            </div>

                            <div className="form-group m-0">
                                <input type="submit" value="Criar" className="btn btn-light m-2" />
                                <button type="button" className="btn btn-info m-2" onClick={() => window.history.back()}>
                                    Voltar à lista!
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Recipescreate;