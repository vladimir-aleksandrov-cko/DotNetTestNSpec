using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DotNetTestNSpec.Configuration;
using DotNetTestNSpec.Domain;
using DotNetTestNSpec.Logging;
using Newtonsoft.Json;
using NSpec;
using NSpec.Api.Discovery;
using NSpec.Api.Execution;
using NSpec.Domain;
using NSpec.Domain.Formatters;

namespace TestApp
{

    class ConsoleLogger : ITestLogger
    {
        public void Error(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string message, Exception ex)
        {
            Console.WriteLine(message);
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Warning(string message)
        {
            Console.WriteLine(message);
        }

        public void Warning(string message, Exception ex)
        {
            Console.WriteLine(message);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            BinaryPathDiscoverer discoverer = new BinaryPathDiscoverer(
                new ConsoleLogger(),
                AdapterSettings.Default);


            var assemblyLocation = Assembly.GetExecutingAssembly().Location;


            Console.WriteLine(assemblyLocation);
            Console.WriteLine(File.Exists(assemblyLocation));

            var testCases = discoverer.Discover(assemblyLocation).ToList();
        }
    }
}
