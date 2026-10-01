import { createContext, useContext, useEffect, useState } from "react";
import { useToast } from "./ToastContext";

const CartContext = createContext(null);

export function CartProvider({ children }) {
  const { showToast } = useToast();
  const [items, setItems] = useState(() => {
    const stored = localStorage.getItem("cart");
    return stored ? JSON.parse(stored) : [];
  });
  const [restaurantId, setRestaurantId] = useState(
    () => localStorage.getItem("cartRestaurantId") || null
  );

  useEffect(() => {
    localStorage.setItem("cart", JSON.stringify(items));
  }, [items]);

  const addItem = (dish, restaurantIdOfDish) => {
    if (restaurantId && restaurantId !== String(restaurantIdOfDish) && items.length > 0) {
      const confirmSwitch = window.confirm(
        "Votre panier contient des plats d'un autre restaurant. Le vider et ajouter ce plat ?"
      );
      if (!confirmSwitch) return;
      setItems([]);
    }
    setRestaurantId(String(restaurantIdOfDish));
    localStorage.setItem("cartRestaurantId", String(restaurantIdOfDish));

    setItems((prev) => {
      const existing = prev.find((i) => i.dishId === dish.id);
      if (existing) {
        return prev.map((i) =>
          i.dishId === dish.id ? { ...i, quantity: i.quantity + 1 } : i
        );
      }
      return [
        ...prev,
        { dishId: dish.id, name: dish.name, unitPrice: dish.price, quantity: 1 },
      ];
    });

    showToast(`${dish.name} ajouté au panier !`);
  };

  const updateQuantity = (dishId, quantity) => {
    if (quantity <= 0) {
      removeItem(dishId);
      return;
    }
    setItems((prev) =>
      prev.map((i) => (i.dishId === dishId ? { ...i, quantity } : i))
    );
  };

  const removeItem = (dishId) => {
    setItems((prev) => prev.filter((i) => i.dishId !== dishId));
  };

  const clearCart = () => {
    setItems([]);
    setRestaurantId(null);
    localStorage.removeItem("cartRestaurantId");
  };

  const total = items.reduce((sum, i) => sum + i.unitPrice * i.quantity, 0);

  return (
    <CartContext.Provider
      value={{
        items,
        restaurantId,
        addItem,
        updateQuantity,
        removeItem,
        clearCart,
        total,
      }}
    >
      {children}
    </CartContext.Provider>
  );
}

export function useCart() {
  return useContext(CartContext);
}