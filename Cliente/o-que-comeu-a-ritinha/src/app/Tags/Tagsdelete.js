import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { deleteTagAPI, getTagAPI } from '../../api/Tagsapi';

function Tagsdelete() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [tag, setTag] = useState(null);

    useEffect(() => {
        getTagAPI(id)
            .then(response => response.json())
            .then(data => setTag(data))
            .catch(error => console.error('Erro ao buscar a tag:', error));
    }, [id]);

    const handleDelete = () => {
        deleteTagAPI(id)
            .then(response => response.text())
            .then(result => {
                console.log(result);
                navigate('/tags'); // Redireciona para a página de tags após a exclusão
            })
            .catch(error => console.error('Erro ao apagar a tag:', error));
    };

    return (
        <div id="backcolor" className='text-center'>
            <h1 className="text-white">Apagar</h1>
            <h3 className="text-white">Tens a certeza que queres apagar esta tag?</h3>
            {tag ? (
                <h3 className="text-black bg-white m-3 p-2 rounded">{tag.tag}</h3>
            ) : (
                <p className="text-white">Carregando...</p>
            )}
            <button onClick={handleDelete} className="btn btn-danger m-2">Apagar</button>
            <Link to={`/tags`} className="btn btn-info m-2">Voltar à lista!</Link>
        </div>
    );
}

export default Tagsdelete;