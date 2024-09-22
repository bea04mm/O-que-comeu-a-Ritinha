import React, { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import { getTagAPI } from '../../api/Tagsapi';

function Tagsdetails() {
    const { id } = useParams();
    const [tag, setTag] = useState('');

    useEffect(() => {
        getTagAPI(id)
            .then(response => response.json())
            .then(data => {
                setTag(data);
            })
            .catch(error => {
                console.error('Erro ao buscar a tag:', error);
            });
    }, [id]);

    return (
        <div id="backcolor" className='text-center'>

            <h1 class="text-white">Detalhes</h1>

            <h3 class="text-black bg-white m-3 p-2 rounded">{tag.tag}</h3>

            <div class="form-group m-0">
                <Link to={`/Tags/Edit/${tag.id}`} className="btn btn-light m-2">Editar</Link>
                <button type="button" className="btn btn-info m-2" onClick={() => window.history.back()}>
                    Voltar à Lista!
                </button>
            </div>
        </div>
    );
}

export default Tagsdetails;
