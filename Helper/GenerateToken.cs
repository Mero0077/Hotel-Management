using Hotel_Management.Data;
using Hotel_Management.DTOs.Account;
using Hotel_Management.Models.Enums;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Hotel_Management.Helper
{
    public class GenerateToken
    {
        public static string Generate (int UserId, string Name,Role Role)
        {
            var key = System.Text.Encoding.ASCII.GetBytes(Hotel_Management.Data.Constants.SecretKey);
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim("ID",UserId.ToString()),
                    new System.Security.Claims.Claim(ClaimTypes.Name,Name),
                    new System.Security.Claims.Claim(ClaimTypes.Role,((int)Role).ToString())

                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials
                (new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature),
                Issuer = Constants.Issuer,
                Audience = Constants.Audience,

            };

            var token= tokenHandler.CreateToken (tokenDescriptor);
            return tokenHandler.WriteToken (token);
            }
        }
    }

