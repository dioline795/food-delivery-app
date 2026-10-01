namespace LivraisonAPI.Models.Enums;

// Correspond à l'enum PostgreSQL "orders_status"
// Les 5 statuts de ta barre de progression de suivi de commande
public enum OrderStatus
{
    Pending,     // En attente
    Confirmed,   // Confirmée
    Preparing,   // En préparation
    OnTheWay,    // En livraison
    Delivered    // Livrée
}
