import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useCart } from "../context/CartContext";
import { useAuth } from "../context/AuthContext";
import { orderApi } from "../api/endpoints";

export default function Cart() {
  const { items, restaurantId, updateQuantity, removeItem, clearCart, total } = useCart();
  const { user } = useAuth();
  const navigate = useNavigate();

  const [address, setAddress] = useState(user?.address || "");
  const [paymentMethod, setPaymentMethod] = useState("cash"); // "cash" ou "card" (paiement simulé)
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState(null);

  const handleCheckout = async () => {
    if (!user) {
      navigate("/login");
      return;
    }
    if (!address.trim()) {
      setError("Merci de renseigner une adresse de livraison.");
      return;
    }
    setSubmitting(true);
    setError(null);
    try {
      const payload = {
        restaurantId: Number(restaurantId),
        deliveryAddress: address,
        notes: `Paiement : ${paymentMethod === "cash" ? "espèces" : "carte bancaire"}`,
        items: items.map((i) => ({ dishId: i.dishId, quantity: i.quantity })),
      };
      const { data } = await orderApi.create(payload);
      clearCart();
      navigate(`/orders/${data.id}`);
    } catch (err) {
      setError("Impossible de valider la commande. Réessayez.");
    } finally {
      setSubmitting(false);
    }
  };

  if (items.length === 0) {
    return (
      <div className="page">
        <h1>Panier</h1>
        <p>Votre panier est vide.</p>
      </div>
    );
  }

  return (
    <div className="page">
      <h1>Panier</h1>
      <div className="cart-list">
        {items.map((item) => (
          <div className="cart-row" key={item.dishId}>
            <span>{item.name}</span>
            <div className="cart-qty">
              <button onClick={() => updateQuantity(item.dishId, item.quantity - 1)}>-</button>
              <span>{item.quantity}</span>
              <button onClick={() => updateQuantity(item.dishId, item.quantity + 1)}>+</button>
            </div>
            <span>{(item.unitPrice * item.quantity).toFixed(2)} ₽</span>
            <button onClick={() => removeItem(item.dishId)}>Supprimer</button>
          </div>
        ))}
      </div>

      <h3>Total : {total.toFixed(2)} ₽</h3>

      <div className="checkout-form">
        <label>
          Adresse de livraison
          <input value={address} onChange={(e) => setAddress(e.target.value)} placeholder="Votre adresse" />
        </label>

        <label>Mode de paiement</label>
        <div className="payment-options">
          <label>
            <input
              type="radio"
              checked={paymentMethod === "cash"}
              onChange={() => setPaymentMethod("cash")}
            />
            Espèces à la livraison
          </label>
          <label>
            <input
              type="radio"
              checked={paymentMethod === "card"}
              onChange={() => setPaymentMethod("card")}
            />
            Carte bancaire
          </label>
        </div>

        {error && <p className="error">{error}</p>}

        <button onClick={handleCheckout} disabled={submitting}>
          {submitting ? "Validation..." : "Valider la commande"}
        </button>
      </div>
    </div>
  );
}
