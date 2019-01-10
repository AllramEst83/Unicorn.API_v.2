using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.API.Entities;
using Auth.API.Entities.Context;
using Auth.API.Helpers;
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

        public UnicornUser Authenticate(string username, string password)
        {
            UnicornUser _user = _context.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

            if (_user == null)
                return null;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                   new Claim(ClaimTypes.Name, _user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            _user.Token = tokenHandler.WriteToken(token);

            _user.Password = null;

            return _user;

        }

        public void DecryptToken(string token)
        {

            string secret = _configuration.GetSection("AppSettings")["Secret"];
            var key = Encoding.ASCII.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();
            var tokenSecure = handler.ReadToken(token) as SecurityToken;
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var claims = handler.ValidateToken(token, validations, out tokenSecure);
            var name = claims.Identity.Name;
        }

        public IEnumerable<object> GetAll()
        {
            return _context.Users.Select(x => new { x.Username }).ToList();
        }
    }
}
