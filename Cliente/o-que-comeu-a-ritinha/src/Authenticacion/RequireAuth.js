import React, { useContext } from 'react';
import { Navigate } from 'react-router-dom';
import { AuthContext } from './AuthContext';

const RequireAuth = ({ children }) => {
    const { user } = useContext(AuthContext);

    // If the user is not logged in, redirect to login page
    if (!user) {
        return <Navigate to="/login" />;
    }

    return children; // If logged in, render the children components
};

export default RequireAuth;