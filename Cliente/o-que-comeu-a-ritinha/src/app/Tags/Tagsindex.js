import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { getTagsAPI } from '../../api/Tagsapi';

function Tagsindex() {
    const [tags, setTags] = useState([]);

    useEffect(() => {
        // Fetch tags da API
        getTagsAPI()
            .then(response => response.json())
            .then(data => {
                setTags(data); // Atualiza o estado com os dados recebidos
            })
            .catch(error => {
                console.error('Erro ao buscar as tags:', error);
            });
    }, []);

    return (
        <div id="backcolor">
            <h1 className="text-center text-white m-0">Tags</h1>
            <p className="text-center m-4">
                <a href="tags/criar" className="btn btn-light">Criar nova Tag!</a>
            </p>
            <div className="justify-content-center">
                <table className="table table-custom text-center m-0">
                    <thead>
                        <tr>
                            <th>Tag</th>
                            <th>Ações</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tags.map(tag => (
                            <tr key={tag.id}>
                                <td>{tag.tag}</td>
                                <td>
                                    <Link to={`/tags/editar/${tag.id}`} className="btn btn-outline-secondary btn-sm btn-block m-1">Editar</Link>
                                    <Link to={`/tags/apagar/${tag.id}`} className="btn btn-outline-danger btn-sm btn-block m-1">Apagar</Link>
                                    <Link to={`/tags/${tag.id}`} className="btn btn-outline-info btn-sm btn-block m-1">Detalhes</Link>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}

export default Tagsindex;