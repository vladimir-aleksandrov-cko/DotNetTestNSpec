using System;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.Api.Discovery;

namespace DotNetTestNSpec.Domain
{
    public static class MappingExtensions
    {
        public static TestCase ToTestCase(this DiscoveredExample example)
        {
            return new TestCase
            {
                FullyQualifiedName = example.FullName,
                ExecutorUri = new Uri(Constants.ExecutorUriString),
                Source = example.SourceAssembly,
                DisplayName = example.FullName,
                CodeFilePath = example.SourceFilePath,
                LineNumber = example.SourceLineNumber
            };
        }
    }
}