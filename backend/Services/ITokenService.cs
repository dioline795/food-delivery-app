using LivraisonAPI.Models;

namespace LivraisonAPI.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
