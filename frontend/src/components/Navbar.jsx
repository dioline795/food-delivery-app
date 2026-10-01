import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { useCart } from "../context/CartContext";

export default function Navbar() {
  const { user, logout, isAdmin } = useAuth();
  const { items } = useCart();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  const itemCount = items.reduce((sum, i) => sum + i.quantity, 0);

  return (
    <nav className="navbar">
      <Link to="/" className="navbar-brand">🍔 FoodDelivery</Link>
      <div className="navbar-links">
        <Link to="/">Accueil</Link>
        <Link to="/cart">Panier {itemCount > 0 && `(${itemCount})`}</Link>
        {user && <Link to="/orders">Mes commandes</Link>}
        {isAdmin && <Link to="/admin">Administration</Link>}
        {user ? (
          <>
            <span className="navbar-user">{user.name}</span>
            <button onClick={handleLogout}>Déconnexion</button>
          </>
        ) : (
          <>
            <Link to="/login">Connexion</Link>
            <Link to="/register">Inscription</Link>
          </>
        )}
      </div>
    </nav>
  );
}
