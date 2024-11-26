using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("getallacc")]
        public async Task<ActionResult<IEnumerable<Accelerometer>>> GetAccelerometerData()
        {
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
        [HttpPost("createacc")]
        public async Task<ActionResult<Accelerometer>> PostAccelerometerData(Accelerometer accdata)
        {
            var newdata = new Accelerometer()
            {
                Id = Guid.NewGuid().ToString("N"),
                X = accdata.X,
                Y = accdata.Y,
                Z = accdata.Z
                // Add any other properties if applicable
            };
            _dbContext.Accelerometer.Add(newdata);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccelerometerData), new { id = newdata.Id }, newdata);
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

