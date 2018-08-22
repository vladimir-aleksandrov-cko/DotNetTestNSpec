using DotNetTestNSpec.Domain;
using DotNetTestNSpec.Shared;
using Newtonsoft.Json;
using NSpec.Api;
using NSpec.Api.Discovery;
using NSpec.Api.Execution;
using NSpec.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetTestNSpec.Domain
{
    public class NspecController
    {
        public NspecController() => controller = new NSpec.Api.Controller();

        public int Run(
            string testAssemblyPath,
            string tags,
            string formatterClassName,
            IDictionary<string, string> formatterOptions,
            bool failFast)
        {
            return controller.Run(
                 testAssemblyPath,
                 tags,
                 formatterClassName,
                 formatterOptions,
                 failFast);
        }

        public IEnumerable<DiscoveredExample> List(string testAssemblyPath)
        {
            return new ExampleSelector(testAssemblyPath).Start();
        }

        public void RunInteractive(
            string testAssemblyPath,
            IEnumerable<string> exampleFullNames,
            Action<DiscoveredExample> onExampleStarted,
            Action<ExecutedExample> onExampleCompleted)
        {
            var exampleRunner = new ExampleRunner(testAssemblyPath, onExampleStarted, onExampleCompleted);
            exampleRunner.Start(exampleFullNames);
        }

        readonly Controller controller;
    }
}