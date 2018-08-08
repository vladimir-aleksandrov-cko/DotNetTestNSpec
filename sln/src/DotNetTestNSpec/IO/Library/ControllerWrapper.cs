using DotNetTestNSpec.Domain;
using DotNetTestNSpec.Domain.Library;
using DotNetTestNSpec.Shared;
using Newtonsoft.Json;
using NSpec.Api;
using NSpec.Api.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetTestNSpec.IO.Library
{
    public class ControllerWrapper : IController
    {
        public ControllerWrapper(Assembly nspecLibraryAssembly)
        {
            controller = new NSpec.Api.Controller();
        }

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
            IExecutionSink sink)
        {
            Action<string> onExampleStarted = jsonArg => OnExampleStarted(sink, jsonArg);
            Action<string> onExampleCompleted = jsonArg => OnExampleCompleted(sink, jsonArg);

            controller.RunInteractive(
                testAssemblyPath, 
                exampleFullNames, 
                onExampleCompleted, 
                onExampleCompleted);
        }

        static void OnExampleStarted(IExecutionSink sink, string jsonArg)
        {
            DiscoveredExample example;

            try
            {
                example = JsonConvert.DeserializeObject<DiscoveredExample>(jsonArg);
            }
            catch (Exception ex)
            {
                throw new DotNetTestNSpecException(unknownArgumentErrorMessage
                    .With(runInteractiveMethodName + ": " + nameof(OnExampleStarted), jsonArg), ex);
            }

            sink.ExampleStarted(example);
        }

        static void OnExampleCompleted(IExecutionSink sink, string jsonArg)
        {
            ExecutedExample example;

            try
            {
                example = JsonConvert.DeserializeObject<ExecutedExample>(jsonArg);
            }
            catch (Exception ex)
            {
                throw new DotNetTestNSpecException(unknownArgumentErrorMessage
                    .With(runInteractiveMethodName + ": " + nameof(OnExampleCompleted), jsonArg), ex);
            }

            sink.ExampleCompleted(example);
        }

        static object InvokeMethod(object controller, string methodName, params object[] args)
        {
            var controllerType = controller.GetType();

            var methodInfo = controllerType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

            if (methodInfo == null)
            {
                throw new DotNetTestNSpecException(unknownMethodErrorMessage.With(methodName));
            }

            object result = methodInfo.Invoke(controller, args);

            return result;
        }

        readonly Controller controller;

        const string runInteractiveMethodName = "RunInteractive";

        const string unknownMethodErrorMessage =
            "Could not find known method ({0}) in referenced NSpec assembly: " +
            "please double check version compatibility between this runner and referenced NSpec library.";
        const string unknownResultErrorMessage =
            "Could not convert serialized result from known method ({0}) in referenced NSpec assembly: " +
            "please double check version compatibility between this runner and referenced NSpec library." +
            "Result: {1}.";
        const string unknownArgumentErrorMessage =
            "Could not convert serialized argument from known callback ({0}) in referenced NSpec assembly: " +
            "please double check version compatibility between this runner and referenced NSpec library." +
            "Argument: {1}.";
    }
}
