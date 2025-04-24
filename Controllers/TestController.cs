using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data.Context;

namespace CantoApi.Controllers
{
    [ApiController]
    [Route("canto-api/v1/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly CantoApiContext _context;

        public TestController(CantoApiContext context)
        {
            _context = context;
        }

        [HttpGet("db-test")]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                // Test database connection
                bool canConnect = await _context.Database.CanConnectAsync();
                
                return Ok(new
                {
                    status = canConnect ? "Connected" : "Cannot Connect",
                    timestamp = DateTime.UtcNow,
                    database = _context.Database.GetDbConnection().Database
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "Error",
                    message = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
