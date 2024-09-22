import React, { useState } from 'react';
import { registerUserAPI } from '../api/Api';
import { useNavigate } from 'react-router-dom';

const Register = () => {
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        Email: '',
        Password: '',
        ConfirmPassword: '',
        Name: '',
        Birthday: '',
        Phone: ''
    });
    const [error, setError] = useState('');

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({
            ...prevData,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        
        const data = new FormData();
        data.append('Email', formData.Email);
        data.append('Password', formData.Password);
        data.append('utilizador.Name', formData.Name);
        data.append('utilizador.Birthday', formData.Birthday);
        data.append('utilizador.Phone', formData.Phone);
    
        try {
            const result = await registerUserAPI(data);
            
            console.log('Resultado do registro:', result);
    
            if (result && result.userId) {
                const newUserId = result.userId;
                // Redirecionar para a página de confirmação
                navigate(`/RegisterConfirmation?userId=${newUserId}`);
            } else {
                setError('Erro ao registrar. Por favor, tente novamente.');
                console.error('UserId não foi encontrado na resposta:', result);
            }
        } catch (error) {
            console.error('Erro no registro:', error);
        }
    };    

    return (
        <div id="backcolor" style={{ textAlign: "center" }}>
            <h1 className="mb-3 text-white">Regista-te</h1>

            {error && <div className="text-danger" role="alert">{error}</div>}

            <form id="registerForm" onSubmit={handleSubmit}>
                <div className="row">
                    <div className="col-md-6">
                        <div className="form-floating mb-3">
                            <input
                                type="email"
                                className="form-control"
                                name="Email"
                                placeholder="name@example.com"
                                value={formData.Email}
                                onChange={handleChange}
                                required
                            />
                            <label>Email</label>
                        </div>

                        <div className="form-floating mb-3">
                            <input
                                type="password"
                                className="form-control"
                                name="Password"
                                placeholder="password"
                                value={formData.Password}
                                onChange={handleChange}
                                required
                            />
                            <label>Password</label>
                        </div>

                        <div className="form-floating mb-3">
                            <input
                                type="password"
                                className="form-control"
                                name="ConfirmPassword"
                                placeholder="Confirmar Password"
                                value={formData.ConfirmPassword}
                                onChange={handleChange}
                                required
                            />
                            <label>Confirmar Password</label>
                        </div>
                    </div>

                    <div className="col-md-6">
                        <div className="form-floating mb-3">
                            <input
                                type="text"
                                className="form-control"
                                name="Name"
                                placeholder="Nome"
                                value={formData.Name}
                                onChange={handleChange}
                                required
                            />
                            <label>Nome</label>
                        </div>

                        <div className="form-floating mb-3">
                            <input
                                type="date"
                                className="form-control"
                                name="Birthday"
                                value={formData.Birthday}
                                onChange={handleChange}
                                required
                            />
                            <label>Data de Nascimento</label>
                        </div>

                        <div className="form-floating mb-3">
                            <input
                                type="text"
                                className="form-control"
                                name="Phone"
                                placeholder="Telefone"
                                value={formData.Phone}
                                onChange={handleChange}
                                required
                            />
                            <label>Telefone</label>
                        </div>
                    </div>

                    <div>
                        <button type="submit" className="m-3 btn btn-lg btn-primary">Registar</button>
                    </div>
                </div>
            </form>
        </div>
    );
};

export default Register;