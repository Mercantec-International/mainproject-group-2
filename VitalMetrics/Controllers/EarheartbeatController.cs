using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VitalMetrics.Data;
using VitalMetrics.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VitalMetrics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EarheartbeatController : ControllerBase
    {
        private readonly AppDBContext _dbContext;

        public EarheartbeatController(AppDBContext context)
        {
            _dbContext = context;
        }

        // GET: api/HeartRateData
        
        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<Earheartbeat>>> GetHeartRateData()
        {
            return await _dbContext.Earheartbeats.ToListAsync();
        }
        [Authorize]
        [HttpGet("getEarheartbeatdata")]
        public async Task<ActionResult<List<Earheartbeat>>> GetUserEarheartbeatData()
        {
            // Retrieve the logged-in user's ID from the JWT token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not logged in.");
            }

            // Fetch Earheartbeat data associated with the logged-in user
            var userEarheartbeatData = await _dbContext.Earheartbeats
                .Where(a => a.UserId == userId) // Filter by UserId
                .AsNoTracking()
                .ToListAsync();

            return Ok(userEarheartbeatData);
        }

        // GET: api/HeartRateData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Earheartbeat>> GetHeartRateData(string Id)
        {
            var data = await _dbContext.Earheartbeats.FindAsync(Id);

            if (data == null)
            {
                return NotFound();
            }

            return data;
        }
        [Authorize]
        // POST: api/HeartRateData
        [HttpPost("create")]
        public async Task<ActionResult<Earheartbeat>> PostHeartRateData(Earheartbeat data)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var newdata = new Earheartbeat()
            {
                Id = Guid.NewGuid().ToString("N"),
                BPM = data.BPM,
                 UserId = data.UserId,
                  CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
                // Add any other properties if applicable
            };
            _dbContext.Earheartbeats.Add(newdata);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetHeartRateData), new { id = newdata.Id }, newdata);
        }

        // PUT: api/HeartRateData/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHeartRateData(string Id, Earheartbeat data)
        {
            if (Id != data.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(data).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HeartRateDataExists(Id))
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

        // DELETE: api/HeartRateData/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHeartRateData(string Id)
        {
            var data = await _dbContext.Earheartbeats.FindAsync(Id);
            if (data == null)
            {
                return NotFound();
            }

            _dbContext.Earheartbeats.Remove(data);
            await _dbContext.SaveChangesAsync();

            return NoContent(); 
        }

        private bool HeartRateDataExists(string Id)
        {
            return _dbContext.Earheartbeats.Any(e => e.Id == Id);
        }
    }
}