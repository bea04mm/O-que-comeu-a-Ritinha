import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
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

function App() {
  return (
    <div className="App">
      <header>
        <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light box-shadow">
          <div className="container-fluid">
            <Link className="navbar-brand text-white" to="/">
              <img src="/images/imageLogo.png" alt="Logo" className="img-fluid rounded-circle" style={{ maxHeight: '100px' }} />
            </Link>
            <button className="navbar-toggler text-white" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
              aria-expanded="false" aria-label="Toggle navigation">
              <span className="navbar-toggler-icon"></span>
            </button>
            <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
              <ul className="navbar-nav flex-grow-1">
                <li className="nav-item">
                  <Link className="nav-link text-white" to="/receitas">Receitas</Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link text-white" to="/acercadenos">Acerca de Nós</Link>
                </li>
                {/*Autenticação */}
                <li className="nav-item">
                  <Link className="nav-link text-white" to="/favoritos">Favoritos</Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link text-white" to="/ingredientes">Ingredientes</Link>
                </li>
                <li className="nav-item">
                  <Link className="nav-link text-white" to="/tags">Tags</Link>
                </li>
              </ul>
              {/* login aqui */}
            </div>
          </div>
        </nav>
      </header>
      <div className="p-5 m-0 flex-fill">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/receitas" element={<Recipes />} />
            <Route path="/acercadenos" element={<About />} />
            <Route path="/favoritos" element={<Favorites />} />
            <Route path="/ingredientes" element={<Ingredientsindex />} />
            <Route path="/ingredientes/:id" element={<Ingredientsdetails />} />
            <Route path="/ingredientes/criar" element={<Ingredientscreate />} />
            <Route path="/ingredientes/editar/:id" element={<Ingredientsedit />} />
            <Route path="/ingredientes/apagar/:id" element={<Ingredientsdelete />} />
            <Route path="/tags" element={<Tagsindex />} />
            <Route path="/tags/:id" element={<Tagsdetails />} />
            <Route path="/tags/criar" element={<Tagscreate />} />
           <Route path="/tags/editar/:id" element={<Tagsedit />} />
           <Route path="/tags/apagar/:id" element={<Tagsdelete />} />
            <Route path="/privacidade" element={<Privacy />} />
          </Routes>
      </div>
      <footer className="footer text-white p-2">
        <div className="container text-center">
          <a href="https://www.facebook.com/oquecomeuaritinha" className="fa-brands fa-facebook text-white text-decoration-none h1 p-2 m-0"></a>
          <a href="https://www.instagram.com/o_que_comeu_a_ritinha" className="fa-brands fa-instagram text-white text-decoration-none h1 p-2 m-0"></a>
          <a href="https://www.youtube.com/channel/UCt3W1lx8yJ14EolFXwAWKKQ" className="fa-brands fa-youtube text-white text-decoration-none h1 p-2 m-0"></a>
        </div>
        <div className="container text-center">
          &copy; 2024 - O que comeu a Ritinha - <Link to="/privacidade">Privacidade</Link>
        </div>
      </footer>
    </div>
  );
}

// componentes de página temporários
function Home() {
  return <h1>Home Page</h1>;
}

function Recipes() {
  return <h1>Recipes Page</h1>;
}

function About() {
  return <h1>About Page</h1>;
}

function Favorites() {
  return <h1>Favorites Page</h1>;
}

function Privacy() {
  return <h1>Privacy Page</h1>;
}

export default App;
