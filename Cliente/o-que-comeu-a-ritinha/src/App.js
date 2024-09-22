import React, { useContext } from 'react';
import { Route, Routes, Link } from 'react-router-dom';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'https://kit.fontawesome.com/c8ef1272a7.js';
import Ingredientsindex from './app/Ingredients/Ingredientsindex';
import Ingredientsdetails from './app/Ingredients/Ingredientsdetails';
import Ingredientscreate from './app/Ingredients/Ingredientscreate';
import Ingredientsdelete from './app/Ingredients/Ingredientsdelete';
import Ingredientsedit from './app/Ingredients/Ingredientsedit';
import Tagsindex from './app/Tags/Tagsindex';
import Tagscreate from './app/Tags/Tagscreate';
import Tagsdetails from './app/Tags/Tagsdetails';
import Tagsedit from './app/Tags/Tagsedit';
import Tagsdelete from './app/Tags/Tagsdelete';
import Recipesindex from './app/Recipes/Recipesindex';
import { URL_IMG } from './api/Api';
import Recipesdetails from './app/Recipes/Recipesdetails';
import Recipesdelete from './app/Recipes/Recipesdelete';
import Home from './app/Home/Home';
import Privacy from './app/Home/Privacy';
import Aboutusindex from './app/Aboutus/Aboutusindex';
import Aboutusedit from './app/Aboutus/Aboutusedit';
import Recipescreate from './app/Recipes/Recipescreate';
import Recipesedit from './app/Recipes/Recipesedit';
import Favorites from './app/Home/Favorites';
import Register from './Authenticacion/Register';
import RegisterConfirmation from './Authenticacion/RegisterConfirmation';
import ConfirmEmail from './Authenticacion/ConfirmEmail';
import Login from './Authenticacion/Login';
import ProtectedRoute from './Authenticacion/ProtectedRoute';
import { AuthContext, AuthProvider } from './Authenticacion/AuthContext';
import RequireAuth from './Authenticacion/RequireAuth';

function App() {
  const { roles, isAuthenticated, logout } = useContext(AuthContext) || {};

  return (
    <div className="App">
      <header>
        <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light box-shadow">
          <div className="container-fluid">
            <Link className="navbar-brand text-white" to="/">
              <img src={`${URL_IMG}/imageLogo.png`} alt="Logo" className="img-fluid rounded-circle" style={{ maxHeight: '100px' }} />
            </Link>
            <button className="navbar-toggler text-white" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
              aria-expanded="false" aria-label="Toggle navigation">
              <span className="navbar-toggler-icon"></span>
            </button>
            <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
              <ul className="navbar-nav flex-grow-1">
                <li className="nav-item">
                  <Link className="nav-link text-white" to="/Recipes">Receitas</Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link text-white" to="/Aboutus">Acerca de Nós</Link>
                </li>
                {/* Autenticação */}
                {isAuthenticated && (
                  <li className="nav-item">
                    <Link className="nav-link text-white" to="/Favorites">Favoritos</Link>
                  </li>)}
                {roles.includes('Admin') && (
                  <>
                    <li className="nav-item">
                      <Link className="nav-link text-white" to="/Ingredients">Ingredientes</Link>
                    </li>
                    <li className="nav-item">
                      <Link className="nav-link text-white" to="/Tags">Tags</Link>
                    </li>
                  </>
                )}
              </ul>
              {/* login aqui */}
              <ul className="navbar-nav">
                {isAuthenticated ? (
                  <>
                    <li className="nav-item">
                      <Link className="nav-link text-white" to="/MySpace"> <span className="fa-solid fa-user h1"></span> </Link>
                    </li>
                    <li className="nav-item">
                      <button className="nav-link text-white btn btn-link p-2 m-0" onClick={logout}> <span className="fa-solid fa-door-open h1"></span> </button>
                    </li>
                  </>
                ) : (
                  <>
                    <li className="nav-item">
                      <Link className="nav-link text-white" to="/Register">Regista-te</Link>
                    </li>
                    <li className="nav-item">
                      <Link className="nav-link text-white" to="/Login">Login</Link>
                    </li>
                  </>
                )}
              </ul>
            </div>
          </div>
        </nav>
      </header>
      <div className="p-5 m-0 flex-fill">
        <AuthProvider>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/Register" element={<Register />} />
            <Route path="/RegisterConfirmation" element={<RegisterConfirmation />} />
            <Route path="/ConfirmEmail/:userId" element={<ConfirmEmail />} />
            <Route path="/Login" element={<Login />} />
            <Route path="/Recipes" element={<Recipesindex />} />
            <Route path="/Recipes/Details/:id" element={<Recipesdetails />} />
            <Route path="/Recipes/Create" element={<ProtectedRoute allowedRoles={['Admin']}><Recipescreate /></ProtectedRoute>} />
            <Route path="/Recipes/Edit/:id" element={<ProtectedRoute allowedRoles={['Admin']}><Recipesedit /></ProtectedRoute>} />
            <Route path="/Recipes/Delete/:id" element={<ProtectedRoute allowedRoles={['Admin']}><Recipesdelete /></ProtectedRoute>} />
            <Route path="/Aboutus" element={<Aboutusindex />} />
            <Route path="/Aboutus/Edit/:id" element={<ProtectedRoute allowedRoles={['Admin']}><Aboutusedit /></ProtectedRoute>} />
            <Route path="/Favorites" element={<RequireAuth><Favorites /></RequireAuth>} />
            <Route path="/Ingredients" element={<ProtectedRoute allowedRoles={['Admin']}><Ingredientsindex /></ProtectedRoute>} />
            <Route path="/Ingredients/Details/:id" element={<ProtectedRoute allowedRoles={['Admin']}><Ingredientsdetails /></ProtectedRoute>} />
            <Route path="/Ingredients/Create" element={<ProtectedRoute allowedRoles={['Admin']}><Ingredientscreate /></ProtectedRoute>} />
            <Route path="/Ingredients/Edit/:id" element={<ProtectedRoute allowedRoles={['Admin']}><Ingredientsedit /></ProtectedRoute>} />
            <Route path="/Ingredients/Delete/:id" element={<ProtectedRoute allowedRoles={['Admin']}><Ingredientsdelete /></ProtectedRoute>} />
            <Route path="/Tags" element={<ProtectedRoute allowedRoles={['Admin']}><Tagsindex /></ProtectedRoute>} />
            <Route path="/Tags/Details/:id" element={<ProtectedRoute allowedRoles={['Admin']}><Tagsdetails /></ProtectedRoute>} />
            <Route path="/Tags/Create" element={<ProtectedRoute allowedRoles={['Admin']}><Tagscreate /></ProtectedRoute>} />
            <Route path="/Tags/Edit/:id" element={<ProtectedRoute allowedRoles={['Admin']}><Tagsedit /></ProtectedRoute>} />
            <Route path="/Tags/Delete/:id" element={<ProtectedRoute allowedRoles={['Admin']}><Tagsdelete /></ProtectedRoute>} />
            <Route path="/Privacy" element={<Privacy />} />
          </Routes>
        </AuthProvider>
      </div>
      <footer className="text-white p-2">
        <div className="container text-center">
          <a href="https://www.facebook.com/oquecomeuaritinha" className="fa-brands fa-facebook text-white text-decoration-none h1 p-2 m-0" aria-label="facebook"></a>
          <a href="https://www.instagram.com/o_que_comeu_a_ritinha" className="fa-brands fa-instagram text-white text-decoration-none h1 p-2 m-0" aria-label="instagram"></a>
          <a href="https://www.youtube.com/channel/UCt3W1lx8yJ14EolFXwAWKKQ" className="fa-brands fa-youtube text-white text-decoration-none h1 p-2 m-0" aria-label="youtube"></a>
        </div>
        <div className="container text-center">
          &copy; 2024 - O que comeu a Ritinha - <Link to="/Privacy">Privacidade</Link>
        </div>
      </footer>
    </div>
  );
}

export default App;