import React, { useState, useContext } from 'react';
import { AuthContext } from './AuthContext';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [rememberMe, setRememberMe] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');
    const { login } = useContext(AuthContext);

    const handleSubmit = async (e) => {
        e.preventDefault();
    
        const formData = new FormData();
        formData.append('Email', email);
        formData.append('Password', password);
        formData.append('RememberMe', rememberMe);
    
        try {
            const response = await fetch('https://o-que-comeu-a-ritinha-server.azurewebsites.net/api/Autentication/Login', {
                method: 'POST',
                body: formData,
            });
        
            const contentType = response.headers.get("content-type");
            let data;
        
            if (contentType && contentType.includes("application/json")) {
                data = await response.json();
            } else {
                const text = await response.text();
                throw new Error(text);
            }
        
            console.log('API Response:', data);
        
            if (response.ok) {
                const userInfo = {
                    email: email,
                    userId: data.userId
                };
            
                login(userInfo, data.roles);
            
                window.location.href = '/'; // Redireciona para a página inicial e faz o reload            
            } else {
                setErrorMessage(data.message || 'Login failed. Please try again.');
            }
        } catch (error) {
            setErrorMessage('Error: ' + error.message);
        }        
    };
    
    return (
        <div id="backcolor" style={{ textAlign: "center" }}>
            <h1 className="mb-3 text-white">Login</h1>
            <div>
                <section>
                    <form id="account" onSubmit={handleSubmit}>
                        {errorMessage && <div className="text-danger" role="alert">{errorMessage}</div>}
                        <div className="form-floating mb-3">
                            <input
                                type="email"
                                className="form-control"
                                placeholder="name@example.com"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                required
                                autoComplete="username"
                            />
                            <label>Email</label>
                        </div>
                        <div className="form-floating mb-3">
                            <input
                                type="password"
                                className="form-control"
                                placeholder="password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required
                                autoComplete="current-password"
                            />
                            <label>Password</label>
                        </div>
                        <div className="checkbox mb-1 text-white">
                            <label className="form-label m-0">
                                <input
                                    type="checkbox"
                                    className="form-check-input"
                                    checked={rememberMe}
                                    onChange={() => setRememberMe(!rememberMe)}
                                />
                                Manter a sessão iniciada?
                            </label>
                        </div>
                        <div>
                            <button id="login-submit" type="submit" className="m-3 btn btn-lg btn-primary">Login</button>
                        </div>
                        <div>
                            <a className="btn btn-light m-2" href="/ForgotPassword">Esqueceste-te da password?</a>
                            <a className="btn btn-primary m-2" href="/Register">Regista-te</a>
                            <a className="btn btn-info m-2" href="/ResendEmailConfirmation">Reenviar confirmação do email</a>
                        </div>
                    </form>
                </section>
            </div>
        </div>
    );
};

export default Login;