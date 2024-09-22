import React, { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { AuthContext } from './AuthContext';

const ProtectedRoute = ({ children, allowedRoles }) => {
    const { roles } = useContext(AuthContext);
    
    const hasAccess = allowedRoles.some(role => roles.includes(role));

    return hasAccess ? children : <Navigate to="/" />;
};

export default ProtectedRoute;