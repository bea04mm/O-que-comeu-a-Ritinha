import React, { useContext, useEffect, useState } from 'react';
import { getAboutusAPI, URL_IMG } from '../../api/Api';
import { AuthContext } from '../../Authenticacion/AuthContext';

function Aboutusindex() {
    const [aboutus, setAboutus] = useState({});

    const { roles } = useContext(AuthContext) || {};

    useEffect(() => {
        getAboutusAPI(1)
            .then(response => response.json())
            .then(data => {
                setAboutus(data);
            })
            .catch(error => {
                console.error('Erro ao buscar a receita:', error);
            });
    }, []);

    if (!aboutus || !aboutus.imageDescription) {
        return <div id="backcolor" className='text-center text-white'>A Carregar...</div>; // Handle loading state or error state here
    }

    return (
        <div id="backcolor" className='text-center'>
            <div className="justify-content-center bg-white m-2 rounded">
                <h1 className="text-black m-0 p-4">Acerca de Nós</h1>

                <div>
                    <img
                        src={`${URL_IMG}/${aboutus.imageDescription}`}
                        alt="Ingride"
                        title="imageDescription"
                        className="img-fluid rounded"
                    />
                    <div className="m-4 p-2" dangerouslySetInnerHTML={{ __html: aboutus.description }} />

                    {roles.includes('Admin') && (
                    <a href={`/Aboutus/Edit/${aboutus.id}`} className="btn btn-info mb-4">Editar</a>
                    )}
                </div>
            </div>
        </div>
    );
}

export default Aboutusindex;