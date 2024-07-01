using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TO_DO___API.Models;

namespace TO_DO___API.Services;

public class TokenService
{
    private readonly SymmetricSecurityKey _securityKey;

    public TokenService(SymmetricSecurityKey securityKey)
    {
        _securityKey = securityKey ?? throw new ArgumentNullException(nameof(securityKey));
    }

    public string GenerateToken(Usuario usuario)
    {
        if (usuario == null)
        {
            throw new ArgumentNullException(nameof(usuario), "Usuario cannot be null");
        }

        Claim[] claims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim("loginTimestamp", DateTime.UtcNow.ToString())
        };

        // Gerando token
        var token = new JwtSecurityToken(
            // Tempo de expiração
            expires: DateTime.Now.AddDays(1),
            // Reeinvidicações (claims)
            claims: claims,
            // Credenciais
            signingCredentials: new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256)
        );

        // Retornando/convertendo o token
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
