using dipSecSerAPI1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace dipSecSerAPI1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DipSecSerContext _context;
        private readonly ILogger _logger;

        public UsersController(DipSecSerContext context, ILogger<User> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Users.AnyAsync(u => u.Login == model.Login))
            {
                return BadRequest("Пользователь уже существует");
            }

            var hashedPassword = HashPassword(model.Password);

            var user = new User
            {
                Surname = model.Surname,
                Name = model.Name,
                Patronymic = model.Patronymic,
                Login = model.Login,
                Password = hashedPassword,
                Phone = model.Phone,
                Email = model.Email,
                RoleId = model.RoleId
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> AuthUser([FromBody] AuthModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
            if (user == null || !VerifyPassword(model.Password, user.Password))
            {
                return Unauthorized(new { message = "Invalid login attempt" });
            }

            return Ok(new { message = "Login successful" });

        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInputPassword = HashPassword(password);
            _logger.LogInformation($"Input Password Hash: {hashedInputPassword}");
            _logger.LogInformation($"Stored Password Hash: {hashedPassword}");
            return hashedInputPassword == hashedPassword;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);

            }
        }


        public class RegisterModel
        {
            public string Surname { get; set; } = null!;
            public string Name { get; set; } = null!;
            public string? Patronymic { get; set; }
            public string Login { get; set; } = null!;
            public string Password { get; set; } = null!;
            public string? Phone { get; set; }
            public string? Email { get; set; }
            public int RoleId { get; set; }
        }

        public class AuthModel
        {
            public string Login { get; set; } = null!;
            public string Password { get; set; } = null!;
        }
    }
}
