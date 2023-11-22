// Usando declarações para incluir os namespaces necessários
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// Namespace onde a classe JwtManager está localizada
namespace Ponto.Configs
{
    // Definição da classe JwtManager
    public class JwtManager
    {
        // Campo privado para armazenar a chave secreta usada para assinar e verificar tokens JWT
        private readonly string secretKey;

        // Construtor da classe que recebe a chave secreta como parâmetro
        public JwtManager(string secretKey)
        {
            this.secretKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjpbImlkYS5jZXViLmJyIiwicG9udG9wcm9mZXNzb3IiXSwibmFtZWlkIjoiU8OpcmdpbyBDb3p6ZXR0aSBCZXJ0b2xkaSBkZSBTb3V6YSIsImdpdmVuX25hbWUiOiJTw6lyZ2lvIENvenpldHRpIEJlcnRvbGRpIGRlIFNvdXphIiwicHJpbWFyeXNpZCI6IjMyODg2NDciLCJDcmVkZW50aWFsIjoie1wiSWRVc3VhcmlvXCI6MzI4ODY0NyxcIk5vbWVcIjpcIlPDqXJnaW8gQ296emV0dGkgQmVydG9sZGkgZGUgU291emFcIixcIkRSVFwiOlwiMDg0MzU0XCJ9IiwibmJmIjoxNjg3MDUxNzY1LCJleHAiOjE2ODcxMzgxNjUsImlhdCI6MTY4NzA1MTc2NSwiaXNzIjoiaHR0cHM6Ly9zZXJ2aWNvcy51bmljZXViLmJyLyJ9.P7IAwxcsdWftTzDGR_WXvxnK1jxPHyD1uYn0YAL7uNc";
        }

        // Método para gerar um token JWT com base nas informações do usuário
        public string GenerateToken(string userId, string userName, int expirationMinutes = 30)
        {
            // Inicializar o manipulador de tokens JWT e a chave de assinatura
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(secretKey);

            // Configurar as reivindicações do token (claims), como o ID do usuário e o nome de usuário
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
            });

            // Configurar a descrição do token, incluindo as reivindicações, tempo de expiração e assinatura
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            // Criar e assinar o token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        // Método para validar e decodificar um token JWT, retornando um ClaimsPrincipal
        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            // Inicializar o manipulador de tokens JWT e a chave de assinatura
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(secretKey);

            // Configurar parâmetros de validação, incluindo a chave de assinatura
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero, // Não permitir variação de tempo
            };

            // Tenta validar e decodificar o token, resultando em um ClaimsPrincipal
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);

            return principal;
        }

        // Método para extrair o valor de uma reivindicação específica do token JWT
        public string GetClaimValueFromToken(string token, string claimType)
        {
            // Obter o ClaimsPrincipal do token usando o método GetPrincipalFromToken
            var principal = GetPrincipalFromToken(token);

            // Buscar a reivindicação desejada no ClaimsPrincipal
            var claim = principal?.FindFirst(claimType)?.Value;

            return claim;
        }
    }
}
