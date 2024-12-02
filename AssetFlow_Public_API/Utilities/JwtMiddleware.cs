using FAMS_Models.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_Public_API.Utilities
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
            //_appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Configuration.AppSettings[Constants.AppSettings.TOKEN_SECRET]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, 
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                
                var userData = jwtToken.Claims.Where(r => r.Type == Constants.CLAIM_USERNAME).FirstOrDefault();
                if(userData != null)
                {
                    string username = userData.Value;
                    var fullname = jwtToken.Claims.Where(r => r.Type == Constants.CLAIM_FULLNAME).Select(r => r.Value).FirstOrDefault();
                    var email = jwtToken.Claims.Where(r => r.Type == Constants.CLAIM_EMAIL).Select(r => r.Value).FirstOrDefault();
                    var isAdmin = jwtToken.Claims.Where(r => r.Type == Constants.CLAIM_IS_ADMIN).Select(r => r.Value).FirstOrDefault();
                    var useAD = jwtToken.Claims.Where(r => r.Type == Constants.CLAIM_USE_AD).Select(r => r.Value).FirstOrDefault();

                    var user = new UserData();
                    user.Username = username;
                    user.Fullname = fullname;
                    user.Email = email;
                    user.IsAdmin = isAdmin;
                    user.UseAD = useAD;
                    user.Token = token;
                    context.Items["User"] = user;
                    
                    // attach user to context on successful jwt validation

                    //var user = userService.GetByUsername(username);

                    //if(user != null)
                    //{
                    //    context.Items["User"] = user;
                    //}
                }
            }
            catch (Exception ex)
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
