import React, { useContext, useEffect, useState } from 'react';
import { AuthContext } from '../../Authenticacion/AuthContext';
import { URL_IMG } from '../../api/Api';

const Recipesindex = () => {
    const [recipes, setRecipes] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [searchString, setSearchString] = useState('');
    const [totalPages, setTotalPages] = useState(0);

    const { roles } = useContext(AuthContext) || {};

    const fetchRecipes = async (page, searchString) => {
        try {
            const response = await fetch(`https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Recipes/GetPagedRecipes?page=${page}&searchString=${searchString}`);
            const data = await response.json();

            console.log(data);

            if (response.ok) {
                setRecipes(data.recipes || []);
                setTotalPages(Math.ceil(data.totalCount / 8));
            } else {
                throw new Error(data.message || 'Error fetching recipes');
            }
        } catch (error) {
            console.error("Fetch error:", error);
        }
    };

    useEffect(() => {
        if (currentPage > 0) {
            fetchRecipes(currentPage, searchString);
        }
    }, [currentPage, searchString]);

    const handlePageChange = (page) => {
        if (page >= 1 && page <= totalPages) {
            setCurrentPage(page);
        }
    };

    return (
        <div id="backcolor" style={{ textAlign: "center" }}>
            <h1 className="text-white m-0">Receitas</h1>
            {roles.includes('Admin') && (
                <p className="text-center m-4">
                    <a href="Recipes/Create" className="btn btn-light">Criar nova Receita!</a>
                </p>
            )}

            {/* Search Bar */}
            <form className="m-4 w-50 mx-auto">
                <input
                    type="text"
                    className="form-control text-center"
                    placeholder="Pesquisar..."
                    onChange={(e) => setSearchString(e.target.value)}
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
                            {roles.includes('Admin') && (
                                <>
                                    <a href={`/Recipes/Edit/${recipe.id}`} className="btn btn-outline-info btn-sm btn-block m-1">Editar</a>
                                    <a href={`/Recipes/Delete/${recipe.id}`} className="btn btn-outline-danger btn-sm btn-block m-1">Apagar</a>
                                </>
                            )}
                        </div>
                    </div>
                ))}
            </div>

            {/* Pagination */}
            <nav aria-label="Page navigation">
                <ul className="pagination justify-content-center">
                    <li className={`page-item ${currentPage === 1 ? 'd-none' : ''}`}>
                        <button className="page-link" onClick={() => handlePageChange(currentPage - 1)} aria-label="Previous">
                            <span aria-hidden="true">&lt;</span>
                        </button>
                    </li>

                    {[...Array(totalPages).keys()].map((page) => (
                        <li key={page + 1} className={`page-item ${currentPage === page + 1 ? 'active' : ''}`}>
                            <button className="page-link" onClick={() => handlePageChange(page + 1)}>
                                {page + 1}
                            </button>
                        </li>
                    ))}

                    <li className={`page-item ${currentPage === totalPages ? 'd-none' : ''}`}>
                        <button className="page-link" onClick={() => handlePageChange(currentPage + 1)} aria-label="Next">
                            <span aria-hidden="true">&gt;</span>
                        </button>
                    </li>
                </ul>
            </nav>
        </div>
    );
};

export default Recipesindex;