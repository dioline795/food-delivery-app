import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { restaurantApi, dishApi } from "../api/endpoints";
import { useCart } from "../context/CartContext";

export default function RestaurantMenu() {
  const { id } = useParams();
  const [restaurant, setRestaurant] = useState(null);
  const [dishes, setDishes] = useState([]);
  const [loading, setLoading] = useState(true);
  const { addItem } = useCart();

  useEffect(() => {
    Promise.all([restaurantApi.getById(id), dishApi.getByRestaurant(id)])
      .then(([resRestaurant, resDishes]) => {
        setRestaurant(resRestaurant.data);
        setDishes(resDishes.data);
      })
      .finally(() => setLoading(false));
  }, [id]);

  if (loading) return <p className="page-state">Chargement...</p>;
  if (!restaurant) return <p className="page-state error">Restaurant introuvable.</p>;

  // Regroupement des plats par catégorie
  const grouped = dishes.reduce((acc, dish) => {
    const catName = dish.categoryName || "Autres";
    if (!acc[catName]) acc[catName] = [];
    acc[catName].push(dish);
    return acc;
  }, {});

  return (
    <div className="page">
      <h1>{restaurant.name}</h1>
      <p>{restaurant.description}</p>

      {Object.entries(grouped).map(([category, catDishes]) => (
        <section key={category}>
          <h2>{category}</h2>
          <div className="dish-list">
            {catDishes.map((dish) => (
              <div className="dish-card" key={dish.id}>
                <div>
                  <h4>{dish.name}</h4>
                  <p>{dish.description}</p>
                  <strong>{dish.price} ₽</strong>
                </div>
                <button
                  disabled={!dish.isAvailable}
                  onClick={() => addItem(dish, restaurant.id)}
                >
                  {dish.isAvailable ? "Ajouter" : "Indisponible"}
                </button>
              </div>
            ))}
          </div>
        </section>
      ))}
    </div>
  );
}
