using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using FAMS_Models.Utilities;
using System.DirectoryServices.AccountManagement;
using FAMS_Public_API;
using FAMS_Models;

namespace FAMS_Public_API.Utilities
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
            var secret = Configuration.AppSettings[Constants.AppSettings.TOKEN_SECRET];
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
            //claims.Add(new Claim(Constants.CLAIM_TRANSPORTER_CODE, string.IsNullOrEmpty(user.Transporter_Code) ? "" : user.Transporter_Code));
            //claims.Add(new Claim(Constants.CLAIM_CUSTOMER_CODE, string.IsNullOrEmpty(user.Customer_Code) ? "" : user.Customer_Code));
            //claims.Add(new Claim(Constants.CLAIM_DRIVER_ID, user.Driver_Id.HasValue ? user.Driver_Id.Value.ToString() : ""));
            //claims.Add(new Claim(Constants.CLAIM_ROLE, user.Role));
            //claims.Add(new Claim(Constants.CLAIM_DEPARTMENT, string.IsNullOrEmpty(user.Department) ? "" : user.Department));
            claims.Add(new Claim(Constants.CLAIM_IS_ADMIN, user.IsAdmin));
            claims.Add(new Claim(Constants.CLAIM_USE_AD, user.UseAD));
            claims.Add(new Claim(Constants.CLAIM_MENU, JsonConvert.SerializeObject(user.Menus)));

            return claims;
        }


    }

}
