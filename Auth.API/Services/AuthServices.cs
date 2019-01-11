using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.API.Auth.API.Models;
using Auth.API.Constants;
using Auth.API.Entities;
using Auth.API.Entities.Context;
using Auth.API.Exstensions;
using Auth.API.Helpers;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace Auth.API.Services
{
    public class AuthServices : IAuthServices
    {
        private AuthContext _context { get; }
        public IConfiguration _configuration { get; }

        private readonly AppSettings _appSettings;

        public AuthServices(
            AuthContext context,
            IOptions<AppSettings> appSettings,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public AuthAPIUser Auth(UnicornUser user)
        {
            UnicornUser _user = UserExists(user.Username, user.Password);
            if (_user == null)
                return null;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            DateTime expirationTime = DateTime.UtcNow.AddSeconds(2);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _appSettings.Host,
                audience: _appSettings.Host,
                claims: GetClaims(_user),
                signingCredentials: GetSigningKey(),
                notBefore: DateTime.UtcNow,
                expires: expirationTime
                );

            var tokenString = tokenHandler.WriteToken(token);
            
            _user.Password = null;

            AuthAPIUser ApiUser = Mapper.Map<AuthAPIUser>(_user);
            ApiUser.Token = tokenString;
            ApiUser.TokenExpiration = expirationTime.ToString("yyyy-MM-dd hh:mm:ss");

            return ApiUser;
        }
        public UserClaimsModel DecodeToken(string tokenString)
        {
            var ValidationParams = new TokenValidationParameters
            {
                ValidIssuer = _appSettings.Host,
                ValidAudience = _appSettings.Host,
                IssuerSigningKey = new SymmetricSecurityKey(GetKey()),
                ValidateLifetime = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
            };

             ClaimsPrincipal principal = new ClaimsPrincipal();

            try
            {

                var handler = new JwtSecurityTokenHandler();
                var validatedToken = handler.ReadToken(tokenString) as SecurityToken;
                principal = handler.ValidateToken(tokenString, ValidationParams, out validatedToken);

            }
            catch (SecurityTokenException ex)
            {
              return new UserClaimsModel() {IsAuthenticated = false };
                 
            }
            catch (Exception e)
            {
                return new UserClaimsModel() { IsAuthenticated = false };
            }

            UserClaimsModel userClaims = GetClaims(principal);


            return userClaims;
        }

        //---

        public UnicornUser UserExists(string username, string password)
        {
            UnicornUser user = _context.Users.SingleOrDefault(x => x.Username == username && x.Password == password);
            return user;
        }


        public bool UserExistsWithRole(string mustHaveRole, UserClaimsModel requestUser)
        {
            var enumRole = mustHaveRole.ToEnum<Roles>();
            UnicornUser user = _context.Users.SingleOrDefault(x => x.Id == requestUser.Id && x.Username == requestUser.UserName && x.Role == enumRole);

            if (user == null)
                return false;

            return true; ;
        }

        public SigningCredentials GetSigningKey()
        {
            byte[] key = GetKey();
            var signedKey = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            return signedKey;
        }

        public byte[] GetKey()
        {
            return Encoding.ASCII.GetBytes(_appSettings.Secret);
        }     
        
        public UserClaimsModel GetClaims(ClaimsPrincipal principal)
        {
            UserClaimsModel userClaims = new UserClaimsModel()
            {
                Id = int.Parse(principal.GetClaimType(AuthServiceConstants.Name)),
                IsAuthenticated = principal.Identity.IsAuthenticated,
                Email = principal.GetClaimType(AuthServiceConstants.Email),
                UserName = principal.GetClaimType(AuthServiceConstants.UserName),
                Role = principal.GetClaimType(AuthServiceConstants.Admin),
                TokenExpiration = principal.GetClaimType(AuthServiceConstants.Expiration)
            };

            return userClaims;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public List<Claim> GetClaims(UnicornUser _user)
        {
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, _user.Id.ToString()),
                new Claim(ClaimTypes.Email, _user.Email,_user.Email),
                new Claim(ClaimTypes.GivenName,_user.Username),
                new Claim(ClaimTypes.Role,_user.Role.ToString())
            };
            return claims;
        }

        public UnicornUser GetUser(string userId)
        {
            var parsedId = int.Parse(userId);
            UnicornUser user = _context.Users.SingleOrDefault(x => x.Id == parsedId);

            return user;
        }
       
        public IEnumerable<object> GetAll()
        {
            return _context.Users.Select(x => new { x.Username }).ToList();
        }
    }
}
