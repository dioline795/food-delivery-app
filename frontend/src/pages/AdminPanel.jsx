import { useEffect, useState } from "react";
import { orderApi } from "../api/endpoints";

const STATUSES = ["Pending", "Confirmed", "Preparing", "OnTheWay", "Delivered"];

export default function AdminPanel() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);

  const loadOrders = () => {
    orderApi.getAll().then((res) => setOrders(res.data));
  };

  useEffect(() => {
    loadOrders();
    setLoading(false);
  }, []);

  const handleStatusChange = async (orderId, newStatus) => {
    await orderApi.updateStatus(orderId, newStatus);
    loadOrders();
  };

  if (loading) return <p className="page-state">Chargement...</p>;

  return (
    <div className="page">
      <h1>Administration — Commandes</h1>
      <table className="admin-table">
        <thead>
          <tr>
            <th>#</th>
            <th>Client</th>
            <th>Restaurant</th>
            <th>Total</th>
            <th>Statut</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((o) => (
            <tr key={o.id}>
              <td>{o.id}</td>
              <td>{o.userName}</td>
              <td>{o.restaurantName}</td>
              <td>{o.totalPrice} ₽</td>
              <td>
                <select value={o.status} onChange={(e) => handleStatusChange(o.id, e.target.value)}>
                  {STATUSES.map((s) => (
                    <option key={s} value={s}>
                      {s}
                    </option>
                  ))}
                </select>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
