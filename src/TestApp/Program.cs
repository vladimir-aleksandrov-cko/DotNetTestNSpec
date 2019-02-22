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

    class MyLogger : ITestLogger
    {
        public void Error(string message)
        {
        }

        public void Error(string message, Exception ex)
        {
        }

        public void Info(string message)
        {
        }

        public void Warning(string message)
        {
        }

        public void Warning(string message, Exception ex)
        {
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            BinaryPathDiscoverer discoverer = new BinaryPathDiscoverer(
                new MyLogger(),
                AdapterSettings.Default);

            var path = @"C:\Git\DotNetTestNSpec-fork\src\TestApp\bin\Debug\netcoreapp2.2\Fomo.Domain.UnitTests.dll";

            var assembly = Assembly.LoadFrom(path);

            var pathExists = File.Exists(path);
            Console.WriteLine(pathExists);

            var testCases = discoverer.Discover(path);



            File.WriteAllText("c://test/examples.txt", JsonConvert.SerializeObject(testCases));
        }
    }
}
