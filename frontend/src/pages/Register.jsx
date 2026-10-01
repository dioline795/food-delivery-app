import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function Register() {
  const [form, setForm] = useState({ name: "", email: "", password: "", phone: "", address: "" });
  const [error, setError] = useState(null);
  const [submitting, setSubmitting] = useState(false);
  const { register } = useAuth();
  const navigate = useNavigate();

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSubmitting(true);
    setError(null);
    try {
      await register(form);
      navigate("/");
    } catch {
      setError("Impossible de créer le compte (email déjà utilisé ?).");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="page auth-page">
      <h1>Inscription</h1>
      <form onSubmit={handleSubmit} className="auth-form">
        <label>
          Nom
          <input name="name" value={form.name} onChange={handleChange} required />
        </label>
        <label>
          Email
          <input type="email" name="email" value={form.email} onChange={handleChange} required />
        </label>
        <label>
          Mot de passe
          <input type="password" name="password" value={form.password} onChange={handleChange} required minLength={6} />
        </label>
        <label>
          Téléphone
          <input name="phone" value={form.phone} onChange={handleChange} />
        </label>
        <label>
          Adresse
          <input name="address" value={form.address} onChange={handleChange} />
        </label>
        {error && <p className="error">{error}</p>}
        <button type="submit" disabled={submitting}>
          {submitting ? "Création..." : "Créer mon compte"}
        </button>
      </form>
    </div>
  );
}
