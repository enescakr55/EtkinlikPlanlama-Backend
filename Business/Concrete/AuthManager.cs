using Business.Abstract;
using Entities.Concrete;
using Entities.Concrete.Dtos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Business.Concrete.Helpers;
using System.Security.Claims;
using Core.Results;
using System.Security.Principal;
using Business.Constants;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly AppSettings _appSettings;

        IUserService _userService;
        public AuthManager(IOptions<AppSettings> appSettings,IUserService userService)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }
        
        public IDataResult<ShowLogin> Login(string email, string password)
        {
            
        User currentUser = _userService.GetByEmailAndPassword(email, password).Data;
            if(currentUser == null)
            {
                return new ErrorDataResult<ShowLogin>(default,Messages.WrongMailOrPassword);
            }
            else
            {
                return new SuccessDataResult<ShowLogin>(GetTokenByEmail(email),Messages.SuccessfulLogin);
            }
            
            
            
        }

        public IDataResult<ShowLogin> RenewToken(int id)
        {
            return new SuccessDataResult<ShowLogin>(GetTokenById(id), Messages.SuccessfulLogin);
        }

        public bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();
            SecurityToken securityToken;
            IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters,out securityToken);
            return true;

        }
        private TokenValidationParameters GetValidationParameters()
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Sample",
                ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(key) // The same key as the one that generate the token
            };
        }
        private ShowLogin GetTokenById(int id)
        {
            var user = _userService.GetByUserId(id);
            return GetTokenByEmail(user.Data.Email);
        }
        private ShowLogin GetTokenByEmail(string email)
        {
            User currentUser = _userService.GetByEmail(email).Data;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, currentUser.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, currentUser.Email));
            //claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            //claims.Add(new Claim(ClaimTypes.Role, "User"));
            //claims.Add(new Claim(ClaimTypes.Role, "Moderator"));
            var expires = DateTime.UtcNow.AddMinutes(30);
            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(claims),

                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var loginInfo = new ShowLogin()
            {
                Email = currentUser.Email,
                Firstname = currentUser.Firstname,
                Lastname = currentUser.Lastname,
                Token = tokenHandler.WriteToken(token),
                TokenExpiration = expires,
                UserId = currentUser.UserId
            };
            return loginInfo;
        }
    }
}
