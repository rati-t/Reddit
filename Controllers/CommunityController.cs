using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reddit.Dtos;
using Reddit.Mapper;
using Reddit.Models;

namespace Reddit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly ApplcationDBContext _context;
        private readonly IMapper _mapper;

        public CommunityController(ApplcationDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Community>>> GetCommunities()
        {
            return await _context.Communities.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Community>> GetCommunity(int id)
        {
            var Community = await _context.Communities.FindAsync(id);

            if (Community == null)
            {
                return NotFound();
            }

            return Community;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommunity(int id, Community Community)
        {
            if (id != Community.Id)
            {
                return BadRequest();
            }

            _context.Entry(Community).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Community>> CreateCommunity(CreateCommunityDto createCommunityDto)
        {
            var Community = _mapper.toCommunity(createCommunityDto);

            _context.Communities.Add(Community);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommunity", new { id = Community.Id }, Community);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunity(int id)
        {
            var Community = await _context.Communities.FindAsync(id);
            if (Community == null)
            {
                return NotFound();
            }

            _context.Communities.Remove(Community);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommunityExists(int id)
        {
            return _context.Communities.Any(e => e.Id == id);
        }
    }
}
