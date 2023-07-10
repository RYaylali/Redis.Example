using StackExchange.Redis;

namespace Redis.Pub.Sub.Subscrible.Example
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("localhost:1453");
            ISubscriber subscriber = redis.GetSubscriber();
            await subscriber.SubscribeAsync("mychannel", (channel, value) =>
              {
                  Console.WriteLine(value);
              });
            Console.Read();
        }
    }
}