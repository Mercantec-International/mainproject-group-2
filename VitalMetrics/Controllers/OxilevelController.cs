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
    public class OxilevelController : ControllerBase
    {
        private readonly AppDBContext _dbContext;
        public OxilevelController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }



        // GET: api/<OxilevelController>
        [Authorize]
        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<Oxilevel>>> GetSensorData()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get user ID from JWT token
            return await _dbContext.OxygenLevel.ToListAsync();
        }

        // GET api/<OxilevelController>/5
        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<Oxilevel>> GetSensorData(int id)
        {
            var sensorData = await _dbContext.OxygenLevel.FindAsync(id);

            if (sensorData == null)
            {
                return NotFound();
            }

            return sensorData;
        }

        // POST api/<OxilevelController>
        [HttpPost("post")]
        public async Task<ActionResult<Oxilevel>> PostSensorData(Oxilevel sensorData)
        {
            var newOxi = new Oxilevel
            {
                Id = Guid.NewGuid().ToString("N"),
                OxygenLevel = sensorData.OxygenLevel,
                HeartRateBPM = sensorData.HeartRateBPM
            };

            _dbContext.OxygenLevel.Add(newOxi);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSensorData), new { id = newOxi.Id }, newOxi);
        }

        // PUT api/<OxilevelController>/5
        [HttpPut("updatebyid/{id}")]
        public async Task<IActionResult> PutSensorData(string Id, Oxilevel sensorData)
        {
            if (Id != sensorData.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(sensorData).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorDataExists(Id))
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

        // DELETE api/<OxilevelController>/5
        [HttpDelete("deletebyid/{id}")]
        public void Delete(int id)
        {
        }
        private bool SensorDataExists(string id)
        {
            return _dbContext.OxygenLevel.Any(e => e.Id == id);
        }
    }
}
