using System;
using System.Text.RegularExpressions;
using DotNetTestNSpec.Configuration;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.Api.Discovery;
using NSpec.Api.Execution;

namespace DotNetTestNSpec.Domain
{
    public static class Extensions
    {
        public static string Beautify(this string fullName)
        {
            // beautification idea taken from
            // https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/TestCaseDTO.cs


            // chop leading, redundant 'nspec. ' context, if any

            var displayName = prefixRegex.Replace(fullName, prefixReplacement);

            // replace context separator

            displayName = separatorRegex.Replace(displayName, separatorReplacement);

            return displayName;
        }

        public static TestOutcome ToTestOutcome(this ExecutedExample example)
        {
            if (example.Failed) return TestOutcome.Failed;
            if (example.Pending) return TestOutcome.Skipped;
            return TestOutcome.Passed;
        }

        public static string ToPrintableString(this TestCase testCase)
        {
            return $@"FQDN: {testCase.FullyQualifiedName} 
                      ExecutorUri: {testCase.ExecutorUri}
                      Source: {testCase.Source}
                      DisplayName: {testCase.DisplayName}
                      CodeFilePath: {testCase.CodeFilePath}
                      LineNumber: {testCase.LineNumber}";
        }


        static Regex prefixRegex = new Regex(@"^nspec\. ");
        static Regex separatorRegex = new Regex(@"\. ");

        const string prefixReplacement = "";
        const string separatorReplacement = " › ";
    }
}