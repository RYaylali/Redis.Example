using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Distributed.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IDistributedCache _distributedCache;

        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        [HttpGet("Set")]
        public async Task<IActionResult> Set(string name, string surName)
        {
            await _distributedCache.SetStringAsync("Name", name, options: new()
            {
                AbsoluteExpiration= DateTime.Now.AddSeconds(30),
                SlidingExpiration=TimeSpan.FromSeconds(3)
            });
            await _distributedCache.SetAsync("Surname", Encoding.UTF8.GetBytes(surName), options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(3)
            });
            return Ok();
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var name = await _distributedCache.GetStringAsync("Name");
            var surnameBinary = await _distributedCache.GetAsync("Surname");
            var surname = Encoding.UTF8.GetString(surnameBinary);
            return Ok(new
            {
                name,
                surname
            });
        }
    }
}
