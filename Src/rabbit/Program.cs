using System;
using System.IO;
using System.Reflection;

namespace rabbit
{
    class Program
    {
        private const string DefaultEndpoint = "http://localhost:4001";
        private const string DefaultWatchFolder = "./";
        static void Main(string[] args)
        {
            var endpoint = DefaultEndpoint;
            var watchFolder = new DirectoryInfo(DefaultWatchFolder).FullName;
            if (args.Length >= 1)
            {
                watchFolder = new DirectoryInfo(args[0]).FullName;
            }
            if (args.Length >= 2)
            {
                endpoint = args[1];
            }

            var asm = Assembly.GetExecutingAssembly();
            var location = new FileInfo(asm.Location).Directory.FullName;
            var host = new NancyAutoHost(endpoint, watchFolder, location);
            host.AutoRun();
            Console.Read();
        }
    }
}
