using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RESTApiNetCore.Models;
using RESTApiNetCore.Helpers;


namespace RESTApiNetCore.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        //IEnumerable<User> GetAll();
    }

    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly TodoContext _context;

        public UserService(IOptions<AppSettings> appSettings, TodoContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;

        }

        public User Authenticate(string username, string password)
        {
            var user = _context.User.SingleOrDefault(x => x.Username == username && x.Password == password);

            // Retorna nulo si no encuentra el usuario
            if (user == null)
                return null;

            // Si se encuentra se genera el jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remover contraseña antes de retornar
            user.Password = null;

            return user;
        }

        //public IEnumerable<User> GetAll()
       // {
         //    var list = await _context.User.ToListAsync();
         //   return list;
        //}
    }
}
