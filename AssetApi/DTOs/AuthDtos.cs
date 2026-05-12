using System.ComponentModel.DataAnnotations;

namespace AssetApi.DTOs;

public record LoginRequest(
    [Required] string Username,
    [Required] string Password
);

public record LoginResponse(
    string Token,
    DateTime ExpiresAt,
    string Username,
    string Role
);
