using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AngularWebsite.API.Models;
using AngularWebsiteAPI.Data;
using AngularWebsiteAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AngularWebsiteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }    

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister) // ApiController will tell the method where the information is coming from, without it, you must manually tell it with [FromBody]
        {
            // validate request

            userForRegister.Username = userForRegister.Username.ToLower(); // set username to lowercase | ApiController 
            
            if (await _repo.UserExists(userForRegister.Username)) // pass username into userexists function
                return BadRequest("Username already exists"); // if user exists, return bad request
            
            var userToCreate = new User
            {
                Username = userForRegister.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegister.Password); // asynchronously register user with given username 

            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto) 
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value)); // get token key that is set in AppSettings:Token

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); // encrypt the key using the hmacsha512signature hashing algorithm
            
            var tokenDescriptor = new SecurityTokenDescriptor // initialize a security token descriptor to give the token some user specific information
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler(); // initialize JWT token handler

            var token = tokenHandler.CreateToken(tokenDescriptor); // generate the token based on the above details

            return Ok(new {
                token = tokenHandler.WriteToken(token) // write the token
            });
        }
    }
}