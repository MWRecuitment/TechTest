using AssetApi.Models;

namespace AssetApi.Services;

public interface ITokenService
{
    (string token, DateTime expiresAt) GenerateToken(User user);
}
