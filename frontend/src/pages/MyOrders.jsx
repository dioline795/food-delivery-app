import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { orderApi } from "../api/endpoints";

export default function MyOrders() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    orderApi
      .getMyOrders()
      .then((res) => setOrders(res.data))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <p className="page-state">Chargement...</p>;

  return (
    <div className="page">
      <h1>Mes commandes</h1>
      {orders.length === 0 && <p>Vous n'avez pas encore passé de commande.</p>}
      <div className="order-list">
        {orders.map((o) => (
          <Link to={`/orders/${o.id}`} key={o.id} className="order-row">
            <span>Commande #{o.id}</span>
            <span>{o.status}</span>
            <span>{o.totalPrice} ₽</span>
          </Link>
        ))}
      </div>
    </div>
  );
}
