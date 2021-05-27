using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assigment3.Data;
using Assigment3.Models;
using Microsoft.AspNetCore.Authorization;
using static BCrypt.Net.BCrypt;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Assigment3.Controllers
{
    [ApiController]

    public class UsersController : Controller
    {
        const int BcryptWorkfactor = 11;
        private readonly Assigment3Context _context;

        public UsersController(Assigment3Context context)
        {
            _context = context;
        }


        //Brugere gettes
        [HttpGet("Users"), AllowAnonymous]
        public async Task<ActionResult<List<User>>> Get()   
        {
            var users = await _context.Users.ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            return users;

        }

        //Get bruger igennem id

        [HttpGet("Register/{id}", Name = "Get"), AllowAnonymous]
        public async Task<ActionResult<User>> Get(string userName)
        {
            var user = await _context.Users.Where(a => a.Name == userName).FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest(new { errorMessage = "Username dosen't exists" });
            }

            return user;
        }

        //Register email
        [HttpPost("register"), AllowAnonymous]
        public async Task<ActionResult<UserLoginDto>> Register(UserLoginDto userReg)
        {
            userReg.Email = userReg.Email.ToLower();
            var emailInUse = await _context.Users.Where(a => a.Email == userReg.Email).FirstOrDefaultAsync();

            if (emailInUse != null)
            {
                return BadRequest(new { errorMessage = "Email is in use" });
            }

            User user = new()
            {
                Email = userReg.Email,
                Name = userReg.Name
            };

            user.HashesPassword = HashPassword(userReg.UserPassword, BcryptWorkfactor);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var jwtToken = new TockenDto
            {
                Token = GenerateToken(user.Name)
            };
           
            return CreatedAtAction("Get", new { id = user.Name }, jwtToken);

        }

        [HttpPost("Login"), AllowAnonymous]
        public ActionResult<TockenDto> Login([FromBody] UserLoginDto loginUser)
        {
            var user = _context.Users.Where(u => u.Name.ToLower() == loginUser.Name.ToLower()).FirstOrDefaultAsync().Result;

            if (user != null)
            {
                var valid = BCrypt.Net.BCrypt.Verify(loginUser.UserPassword, user.HashesPassword);
                if (valid) return new TockenDto { Token = GenerateToken(user.Name) };
            }

            ModelState.AddModelError(string.Empty, "Invalid login!");
            return BadRequest(ModelState);
        }

        private string GenerateToken(string name)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddMinutes(1)).ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Secret Key must be minimum 16 characters")),
                    SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
      
    }
}
