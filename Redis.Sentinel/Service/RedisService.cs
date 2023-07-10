using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Redis.Sentinel.Service
{
    public class RedisService
    {
        static ConfigurationOptions sentinelOptions => new()
        {
            EndPoints =
            {
                {"localhost",6383 },//Bunlar bizim sentinel olarak belirlediğimiz sunuculardır
                {"localhost",6384 },
                {"localhost",6385 }
            },
            CommandMap = CommandMap.Sentinel,//sentinel görevi üslendiklerini belirtiyoruz
            AbortOnConnectFail = false
        };
        static ConfigurationOptions masterOptions => new()
        {

            AbortOnConnectFail = false
        };
        static public async Task<IDatabase> RedisMasterDatabase()
        {
            ConnectionMultiplexer sentinelConnection = await ConnectionMultiplexer.SentinelConnectAsync(sentinelOptions);
            System.Net.EndPoint masterEndPoint = null;
            foreach (System.Net.EndPoint endpoint in sentinelConnection.GetEndPoints())
            {
                IServer server = sentinelConnection.GetServer(endpoint);//Sentinel bağlantısında sorun varsa diğer sentinel e geçmesini sağlar
                if (!server.IsConnected)
                {
                    continue;
                }
                masterEndPoint = await server.SentinelGetMasterAddressByNameAsync("mymaster");
                break;
            }
            var localMasterIP = masterEndPoint.ToString() switch
            {
                "172.17.0.2:6379"=>"localhost:6379",
                "172.17.0.3:6379"=>"localhost:6380"
            };//localimde docker ile oluşturduğum masterin endpointi ve slider(köle)(replication yedekleme)lerin endpoinleri
            ConnectionMultiplexer masterConnection = await ConnectionMultiplexer.ConnectAsync(localMasterIP);
            IDatabase database= masterConnection.GetDatabase();
            return database;
        }
    }
}
