import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { orderApi } from "../api/endpoints";

const STATUS_STEPS = [
  { key: "Pending", label: "En attente" },
  { key: "Confirmed", label: "Confirmée" },
  { key: "Preparing", label: "En préparation" },
  { key: "OnTheWay", label: "En livraison" },
  { key: "Delivered", label: "Livrée" },
];

export default function OrderTracking() {
  const { id } = useParams();
  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchOrder = () => orderApi.getById(id).then((res) => setOrder(res.data));
    fetchOrder().finally(() => setLoading(false));

    // Rafraîchit le statut toutes les 10 secondes (suivi "temps réel")
    const interval = setInterval(fetchOrder, 10000);
    return () => clearInterval(interval);
  }, [id]);

  if (loading) return <p className="page-state">Chargement...</p>;
  if (!order) return <p className="page-state error">Commande introuvable.</p>;

  const currentIndex = STATUS_STEPS.findIndex((s) => s.key === order.status);

  return (
    <div className="page">
      <h1>Commande #{order.id}</h1>

      <div className="progress-bar">
        {STATUS_STEPS.map((step, index) => (
          <div key={step.key} className={`progress-step ${index <= currentIndex ? "done" : ""}`}>
            <div className="progress-dot" />
            <span>{step.label}</span>
          </div>
        ))}
      </div>

      <div className="order-summary">
        <p>Adresse de livraison : {order.deliveryAddress}</p>
        <p>Total : {order.totalPrice} ₽</p>
        <h3>Articles</h3>
        <ul>
          {order.orderItems?.map((item) => (
            <li key={item.id}>
              {item.quantity} × {item.dishName} — {item.unitPrice} ₽
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
}
