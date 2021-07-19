using BenchmarkDotNet.Running;

namespace AllItemsWithSameKey
{
    public class Program
    {
        static void Main(string[] args) => BenchmarkRunner.Run(typeof(Program).Assembly);
    }
}