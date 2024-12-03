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
    public class SensorDataController : ControllerBase
    {
        private readonly AppDBContext _dbContext;

        public SensorDataController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Authorize]
        [HttpGet("GetUnifiedSensorData")]
        public async Task<ActionResult<List<SensorDataResponse>>> GetUnifiedSensorData()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get user ID from JWT token
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not logged in.");
            }

            // Fetch user-specific data
            var oxygenLevels = await _dbContext.OxygenLevel
                .Where(o => o.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            var accelerometers = await _dbContext.Accelerometer
                .Where(a => a.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            var fingerHeartbeats = await _dbContext.Fingerheartbeats
                .Where(f => f.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            // Create unified response
            var unifiedData = new List<SensorDataResponse>();
            int maxRows = Math.Max(oxygenLevels.Count, Math.Max(accelerometers.Count, fingerHeartbeats.Count));

            for (int i = 0; i < maxRows; i++)
            {
                unifiedData.Add(new SensorDataResponse
                {
                    OxygenLevel = i < oxygenLevels.Count ? oxygenLevels[i].OxygenLevel : null,
                    HeartRateBPM = i < oxygenLevels.Count ? oxygenLevels[i].HeartRateBPM : null,
                    Changes = i < accelerometers.Count ? accelerometers[i].Changes : null,
                    X = i < accelerometers.Count ? accelerometers[i].X : null,
                    Y = i < accelerometers.Count ? accelerometers[i].Y : null,
                    Z = i < accelerometers.Count ? accelerometers[i].Z : null,
                    BPM = i < fingerHeartbeats.Count ? fingerHeartbeats[i].BPM : null
                });
            }

            return Ok(unifiedData);
        }
        [Authorize]
        [HttpPost("AddOxygenLevel")]
        public async Task<IActionResult> AddOxygenLevel(Oxilevel oxilevel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;// Assuming JWT contains user ID in the "sub" claim
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not logged in.");
            }

            oxilevel.UserId = userId;
            _dbContext.OxygenLevel.Add(oxilevel);
            await _dbContext.SaveChangesAsync();

            return Ok(oxilevel);
        }
    }
}

