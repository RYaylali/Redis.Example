using StackExchange.Redis;

namespace Redis.Pub.Sub.Example
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            ConnectionMultiplexer redis = await ConnectionMultiplexer.ConnectAsync("localhost:1453");
            ISubscriber subscriber = redis.GetSubscriber();
            while (true)
            {
                Console.WriteLine("Mesaj : ");
                string mesaj = Console.ReadLine();
                await subscriber.PublishAsync("mychannel", mesaj);
            }
        }
    }
}