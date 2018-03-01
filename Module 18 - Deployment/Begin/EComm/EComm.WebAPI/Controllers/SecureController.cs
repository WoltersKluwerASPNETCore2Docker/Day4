using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace EComm.WebAPI.Controllers
{
    public class TokenRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [Produces("application/json")]
    [Route("api/Secure")]
    public class SecureController : Controller
    {
        private IConfiguration Configuration { get; }
        public SecureController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [Authorize]
        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok("Super secret content...");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RequestToken([FromBody] TokenRequest request)
        {
            // Add better authentication system here
            if (request.Username == "username" && request.Password == "password")
            {
                var claims = new[] { new Claim(ClaimTypes.Name, request.Username) };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "geeklearn.com",
                    audience: "geeklearn.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return BadRequest("Could not verify username and password");
        }
    }
}