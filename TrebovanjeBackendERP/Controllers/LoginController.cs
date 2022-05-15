
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TrebovanjeBackendERP.Entities;
using TrebovanjeBackendERP.Models;
using TrebovanjeBackendERP.Repositories;

namespace TrebovanjeBackendERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private IKorisnikRepository korisnikRepository;
        public LoginController(IConfiguration config,IKorisnikRepository korisnikRepository)
        {
            _config = config;
            this.korisnikRepository = korisnikRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        private string Generate(Korisnik korisnik)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            if (korisnik.Admin)
            {
                korisnik.Role = "Admin";
            }
            else
            {
                korisnik.Role = "Distributer";
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, korisnik.KorisnickoIme),
                new Claim(ClaimTypes.Email, korisnik.Email),
                new Claim(ClaimTypes.Role,korisnik.Role)

        };

            

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Korisnik Authenticate(UserLogin userLogin)
        {
            var currentUser = korisnikRepository.GetKorisnikByUsernameAndPassword(userLogin.Username,userLogin.Password);

            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }
    }
}