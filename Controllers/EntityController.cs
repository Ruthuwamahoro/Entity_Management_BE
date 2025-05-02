using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityApi.Data;
using EntityApi.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace EntityApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntityController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EntityController> _logger;

        public EntityController(AppDbContext context, ILogger<EntityController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public record ApiResponse<T>(bool Success, string Message, T Data);

        [HttpPost("add")]
        public async Task<IActionResult> Add(Entity entity)
        {
            try
            {
                _context.Entities.Add(entity);
                await _context.SaveChangesAsync();
                return Ok(new {
                    success=true,
                    message="Entity added successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding entity");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] string name = null, [FromQuery] string phoneNumber = null)
        {
            try
            {
                if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(phoneNumber))
                {
                    return BadRequest(new { success = false, message = "Either name or phone number must be provided" });
                }

                Entity entity = null;
                
                if (!string.IsNullOrEmpty(name))
                {
                    entity = await _context.Entities.FirstOrDefaultAsync(e => e.Name == name);
                }
                else if (!string.IsNullOrEmpty(phoneNumber))
                {
                    entity = await _context.Entities.FirstOrDefaultAsync(e => e.Phone == phoneNumber);
                }

                if (entity == null)
                {
                    return NotFound(new { success = false, message = "Entity not found" });
                }

                _context.Entities.Remove(entity);
                await _context.SaveChangesAsync();
                
                return Ok(new {
                    success = true,
                    message = "Entity deleted successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity");
                return StatusCode(500, new { success = false, message = "Internal Server Error" });
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            var list = await _context.Entities.ToListAsync();
            return Ok(new {
                success=true,
                message="Entities listed successfully",
                data=list
            });
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name = null, [FromQuery] string phoneNumber = null)
        {
            try
            {
                if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(phoneNumber))
                {
                    var allEntities = await _context.Entities.ToListAsync();
                    return Ok(new ApiResponse<List<Entity>>(
                        Success: true,
                        Message: $"Found {allEntities.Count} entities",
                        Data: allEntities
                    ));
                }

                IQueryable<Entity> query = _context.Entities;

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(e => EF.Functions.Like(e.Name.ToLower(), $"%{name.ToLower()}%"));
                }

                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    query = query.Where(e => EF.Functions.Like(e.Phone.ToLower(), $"%{phoneNumber.ToLower()}%"));
                }

                var result = await query.ToListAsync();

                string searchCriteria = !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phoneNumber)
                    ? $"name '{name}' and phone number '{phoneNumber}'"
                    : !string.IsNullOrEmpty(name)
                        ? $"name '{name}'"
                        : $"phone number '{phoneNumber}'";

                return Ok(new ApiResponse<List<Entity>>(
                    Success: true,
                    Message: $"Found {result.Count} entities matching {searchCriteria}",
                    Data: result
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching entities");
                return StatusCode(500, new ApiResponse<object>(
                    Success: false,
                    Message: "Internal Server Error",
                    Data: null
                ));
            }
        }
    }
}
