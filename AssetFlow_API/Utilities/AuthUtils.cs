using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using FAMS_Data;
using FAMS_Models.Utilities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FAMS_API.Utilities
{
    public class AuthUtils
    {
        public static void CreateUserData(UserData user, DateTime tokenDate)
        {
            var claims = GenerateClaims(user);
            var token = GenerateJwtToken(claims, tokenDate);

            user.Token = token;

        }

        private static string GenerateJwtToken(List<Claim> claimList, DateTime tokenDate)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = ConfigurationManager.AppSettings[Constants.AppSettings.TOKEN_SECRET];
            var key = Encoding.ASCII.GetBytes(secret);

            var claims = new ClaimsIdentity();
            claims.AddClaims(claimList);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = tokenDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static List<Claim> GenerateClaims(UserData user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Username));
            claims.Add(new Claim(Constants.CLAIM_USERNAME, user.Username));
            claims.Add(new Claim(Constants.CLAIM_FULLNAME, user.Fullname));
            claims.Add(new Claim(Constants.CLAIM_EMAIL, string.IsNullOrEmpty(user.Email) ? "" : user.Email));
            claims.Add(new Claim(Constants.CLAIM_IS_ADMIN, user.IsAdmin));
            claims.Add(new Claim(Constants.CLAIM_USE_AD, user.UseAD));
            claims.Add(new Claim(Constants.CLAIM_MENU, JsonConvert.SerializeObject(user.Menus)));
            claims.Add(new Claim(Constants.CLAIM_LANGUAGE, string.IsNullOrEmpty(user.Language) ? "EN" : user.Language));
            claims.Add(new Claim(Constants.CLAIM_SERVER, user.SERVER));

            return claims;
        }

        public static void DeactivatePastTokens(DataEntities db, UserData userData)
        {
            var username = userData.Username;

            var pastTokens = db.UserToken.Where(r => r.Username == username && r.Active == "Y").ToArray();
            if (pastTokens.Length > 0)
            {
                for(int i=0; i < pastTokens.Length; i++)
                {
                    var token = pastTokens[i];
                    token.Active = "N";
                    db.Entry(token).State = System.Data.Entity.EntityState.Modified;
                }
            }


        }
    }
}
