import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { restaurantApi } from "../api/endpoints";

export default function Home() {
  const [restaurants, setRestaurants] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    restaurantApi
      .getAll()
      .then((res) => setRestaurants(res.data))
      .catch(() => setError("Impossible de charger les restaurants."))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p className="page-state">Chargement...</p>;
  if (error) return <p className="page-state error">{error}</p>;

  return (
    <div className="page">
      <div className="hero">
        <h1>🍔 Livraison de repas rapide et facile</h1>
        <p>Choisissez un restaurant et commandez en quelques clics</p>
      </div>

      <h2 className="section-title">Restaurants</h2>

      <div className="restaurant-grid">
        {restaurants.map((r) => (
          <div key={r.id} className="restaurant-card">
            <img
              src={r.imageUrl || "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?w=600"}
              alt={r.name}
            />
            <div className="restaurant-info">
              <h3>{r.name}</h3>
              <p>{r.description}</p>
              <div className="restaurant-meta">
                <span>⭐ {r.rating}</span>
                <span>🕒 {r.deliveryTime} min</span>
              </div>
              <Link to={`/restaurant/${r.id}`} className="restaurant-btn">
                Ouvrir le menu
              </Link>
            </div>
          </div>
        ))}
      </div>

      {restaurants.length === 0 && (
        <p style={{ textAlign: "center", color: "#6b7280" }}>
          Aucun restaurant disponible pour le moment.
        </p>
      )}
    </div>
  );
}