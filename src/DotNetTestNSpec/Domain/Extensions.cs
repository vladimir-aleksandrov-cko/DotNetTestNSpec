using System;
using System.Text.RegularExpressions;
using DotNetTestNSpec.Configuration;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSpec.Api.Discovery;

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


        static Regex prefixRegex = new Regex(@"^nspec\. ");
        static Regex separatorRegex = new Regex(@"\. ");

        const string prefixReplacement = "";
        const string separatorReplacement = " › ";
    }
}