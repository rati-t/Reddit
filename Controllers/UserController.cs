using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reddit.Dtos;
using Reddit.JwtHelper;
using Reddit.Mapper;
using Reddit.Models;

namespace Reddit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public UserController(ApplicationDbContext context, JwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor(CreateUserDto createAuthorDto)
        {
            var author = new User
            {
                Name = createAuthorDto.Name
            };

            await _context.Users.AddAsync(author);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAuthors()
        {
            return await _context.Users.ToListAsync();
        }
        [HttpPost("JoinCommunity")]
        public async Task<IActionResult> JoinCommunity(int userId,int communityId)
        {
            var community = await _context.Communities.FindAsync(communityId);

            if (community == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.SubscribedCommunities.Add(community);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Register model)
        {
            var user = new User { UserName = model.Username, Email = model.Email, Password = model.Password };

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return Ok(new { message = "User registered successfully" });
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(item => item.UserName == model.Username);
            if (user != null && user.Password == model.Password)
            {
                var token = _jwtTokenGenerator.GenerateToken(user);
                return Ok(new { token });
            }
            return Unauthorized();
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken(Login model)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == model.Username);
            if (user == null && user.Password != model.Password)
            {
                return Unauthorized();
            }

            var token = _jwtTokenGenerator.GenerateToken(user);
            return Ok(new { token });
        }
    }
}