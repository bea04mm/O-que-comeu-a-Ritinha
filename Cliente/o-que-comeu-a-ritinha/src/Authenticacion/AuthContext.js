import React, { createContext, useState } from 'react';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(() => {
        const savedUserId = localStorage.getItem('userId');
        const savedUser = savedUserId ? { userId: savedUserId } : null;
        return savedUser; // Retorna o userId se estiver no localStorage
    });
    const [roles, setRoles] = useState(() => {
        const savedRoles = localStorage.getItem('roles');
        return savedRoles ? JSON.parse(savedRoles) : [];
    });
    const [isAuthenticated, setIsAuthenticated] = useState(!!localStorage.getItem('userId'));

    const login = (userInfo, userRoles) => {
        setUser(userInfo);
        setRoles(userRoles);
        setIsAuthenticated(true);
        
        // Salva o userId como um token no localStorage
        localStorage.setItem('userId', userInfo.userId); 
        localStorage.setItem('roles', JSON.stringify(userRoles));
    };    

    const logout = () => {
        setUser(null);
        setRoles([]);
        setIsAuthenticated(false);
        localStorage.removeItem('userId');
        localStorage.removeItem('roles');
        window.location.href = '/'; // Redireciona para a página inicial e faz o reload            
    };

    return (
        <AuthContext.Provider value={{ user, roles, isAuthenticated, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};