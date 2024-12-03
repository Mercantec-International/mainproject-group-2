using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VitalMetrics.Data;
using VitalMetrics.Models;

namespace VitalMetrics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccelerometerController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        public AccelerometerController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Authorize]
        [HttpGet("getallacc")]
        public async Task<ActionResult<IEnumerable<Accelerometer>>> GetAccelerometerData()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get user ID from JWT token
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not logged in.");
            }
            return await _dbContext.Accelerometer.ToListAsync();
        }
        [HttpGet("getaccbyid/{id}")]
        public async Task<ActionResult<Accelerometer>> GetAccelerometerById(string id)
        {
            var sensorData = await _dbContext.Accelerometer.FindAsync(id);

            if (sensorData == null)
            {
                return NotFound();
            }

            return sensorData;
        }
        [Authorize]
        [HttpPost("createacc")]
        public async Task<ActionResult<Accelerometer>> PostAccelerometerData(Accelerometer accdata)
        {
            // Retrieve the logged-in user's ID from the JWT token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not logged in.");
            }

            // Ensure the user exists in the database
            var userExists = await _dbContext.Users.AsNoTracking().AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return BadRequest("User not found in the database.");
            }

            // Create new Accelerometer data and associate it with the user
            var newdata = new Accelerometer()
            {
                Id = Guid.NewGuid().ToString("N"),
                X = accdata.X,
                Y = accdata.Y,
                Z = accdata.Z,
                UserId = userId, // Associate with the logged-in user's ID
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save to database
            _dbContext.Accelerometer.Add(newdata);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccelerometerData), new { id = newdata.Id }, newdata);
        }

        [Authorize]
        [HttpGet("getuseraccelerometer")]
        public async Task<ActionResult<List<Accelerometer>>> GetUserAccelerometerData()
        {
            // Retrieve the logged-in user's ID from the JWT token
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not logged in.");
            }

            // Fetch accelerometer data associated with the logged-in user
            var userAccelerometerData = await _dbContext.Accelerometer
                .Where(a => a.UserId == userId) // Filter by UserId
                .AsNoTracking() 
                .ToListAsync();

            return Ok(userAccelerometerData);
        }


        [HttpPut("updateaccbyid/{id}")]
        public async Task<IActionResult> PutAccelerometerData(string Id, Accelerometer data)
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
                if (!AccelerometerDataExists(Id))
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
        [HttpDelete("deleteaccbyid/{id}")]
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
        private bool AccelerometerDataExists(string Id)
        {
            return _dbContext.Accelerometer.Any(e => e.Id == Id);
        }
    }
}

