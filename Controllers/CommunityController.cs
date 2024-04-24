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
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CommunityController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Community>>> GetCommunities(int pageNumber, int pageSize, bool isAscending, string? sortKey, string? searchKey)
        {
            IQueryable<Community> query;
            if (searchKey != null)
                query = _context.Communities.Where(c => c.Name.Contains(searchKey) || c.Description.Contains(searchKey));
            else
                query = _context.Communities;
            if (sortKey != null)
            {
                if (isAscending)
                {
                    if (sortKey.ToLower() == "createdat")
                        query = query.OrderBy(c => c.CreateAt);
                    else if (sortKey.ToLower() == "postscount")
                        query = query.OrderBy(c => c.Posts.Count);
                    else if (sortKey.ToLower() == "subscriberscount")
                        query = query.OrderBy(c => c.Subscribers.Count);
                    else
                        query = query.OrderBy(c => c.Id);
                }else
                {

                    if (sortKey.ToLower() == "createdat")
                        query = query.OrderByDescending(c => c.CreateAt);
                    else if (sortKey.ToLower() == "postscount")
                        query = query.OrderByDescending(c => c.Posts.Count);
                    else if (sortKey.ToLower() == "subscriberscount")
                        query = query.OrderByDescending(c => c.Subscribers.Count);
                    else
                        query = query.OrderByDescending(c => c.Id);
                }
            }
            else
            {
                query = query.OrderBy(c => c.Id);
            }
            var result = await query.Skip(pageSize * pageNumber).Take(pageSize).ToListAsync();
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Community>> GetCommunity(int id)
        {
            var community = await _context.Communities.FindAsync(id);

            if (community == null)
            {
                return NotFound();
            }

            return community;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCommunity(CreateCommunityDto communityDto)
        {
            var community = _mapper.toCommunity(communityDto);

            await _context.Communities.AddAsync(community);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunity(int id)
        {
            var community = await _context.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }

            _context.Communities.Remove(community);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommunity(int id, Community community)
        {
            if (!CommunityExists(id))
            {
                return NotFound();
            }

            _context.Entry(community).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool CommunityExists(int id) => _context.Communities.Any(e => e.Id == id);
    }
}
