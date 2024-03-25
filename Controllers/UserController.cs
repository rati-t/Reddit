using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Reddit.Dtos;
using Reddit.Mapper;
using Reddit.Models;

namespace Reddit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplcationDBContext _context;

        public UserController(ApplcationDBContext context)
        {
            _context = context;
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

        [HttpPut("{id}")]
        public async Task<IActionResult> JoinCommunity(int id, int communityId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);

            if(user == null)
            {
                return NotFound();
            }

            var community = await _context.Communities.FirstOrDefaultAsync(x => x.Id == communityId);

            if (community == null)
            {
                return NotFound();
            }

            user.Communities.Add(community);
            community.Subscribers.Add(user);

            _context.Entry(user).State = EntityState.Modified;
            _context.Entry(community).State = EntityState.Modified;
            await _context.SaveChangesAsync();  

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAuthors()
        {
            return await _context.Users.ToListAsync();
        }
    }
}