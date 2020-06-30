using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CadastroUsuarios.Infra.CrossCutting
{
    public class JWT 
    {
        public static string GerarToken(string issuer, string audience, DateTime expiraEm, string chave)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: issuer,
                                             audience: audience,
                                             expires: expiraEm,
                                             signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
