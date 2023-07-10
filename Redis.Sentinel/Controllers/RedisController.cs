﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Redis.Sentinel.Service;

namespace Redis.Sentinel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        [HttpGet("[action]/{key}/{value}")]
        public async Task<IActionResult> SetValue(string key, string value)
        {
            var redis = await RedisService.RedisMasterDatabase();
            await redis.StringSetAsync(key, value);
            return Ok();
        }
        [HttpGet("[action]/{key}")]
        public async Task<IActionResult> GetValue(string key)
        {
            var redis = await RedisService.RedisMasterDatabase();
            var data = await redis.StringGetAsync(key);
            return Ok(data.ToString());
        }
    }
}
