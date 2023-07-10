using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IMemoryCache _memoryCache;

        public ValuesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        [HttpGet("Set/{name}")]
        public void SetName(string name)
        {
            _memoryCache.Set("name", name);
        }
        [HttpGet]
        public string GetName()
        {
            return _memoryCache.Get<string>("name");
        }
        [HttpGet("TryGetValue")]
        public string GetName1()
        {//trygetvalue normal çalışma zamnaında eğer boş değer gelirse boş dönme hatası vermesini önler
            if (_memoryCache.TryGetValue<string>("name",out string name))
            {
                return name.Substring(3);
            }
            return "";
        }
        [HttpGet("SetDate")]
        public void SetDate()
        {
            _memoryCache.Set<DateTime>("date", DateTime.Now, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),//Veri eklendikten 30 saniye sonra veri silinir
                SlidingExpiration= TimeSpan.FromSeconds(5),//Veri sliding periyodu 5 saniye içinde her hangi bir işlem olmaz ise de silinir
            });
        }
        [HttpGet("GetDate")]
        public DateTime GetDate()
        {
            return _memoryCache.Get<DateTime>("date");
        }
    }
}
