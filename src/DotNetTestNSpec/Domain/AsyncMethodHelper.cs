using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using NSpec;

namespace DotNetTestNSpec.Domain
{
    public class AsyncMethodHelper
    {

        public static string GetClassNameForAsyncMethod(Assembly assembly, string className, string methodName)
        {
            if (assembly == null) return null;

            var definingType = assembly.GetType(className);

            if (definingType == null) return null;

            var methodInfo = FindAsyncMethodByName(definingType, methodName);

            if (methodInfo == null) return null;

            var stateMachineAttribute = FindAsyncStateMachineAttribute(methodInfo);

            if (stateMachineAttribute == null) return null;

            var stateMachineTypeProperty = FindAsyncStateMachineTypeProperty(stateMachineAttribute);

            if (stateMachineTypeProperty == null) return null;

            var stateMachineType = stateMachineTypeProperty.GetValue(stateMachineAttribute, new Object[0]) as Type;

            return stateMachineType == null
                ? null :
                stateMachineType.FullName;
        }

        static MethodInfo FindAsyncMethodByName(Type definingType, string methodName)
        {
            var methodInfo = definingType
                // get all instance methods in hierarchy, apart from private methods on base classes
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                // exclude methods defined on base NSpec class
                .Where(info => info.GetBaseDefinition().DeclaringType != typeof(nspec))
                .Where(info => info.Name == methodName)
                .OrderBy(info => info.GetParameters().Length)
                .FirstOrDefault();

            return methodInfo;
        }

        static Attribute FindAsyncStateMachineAttribute(MethodInfo method)
        {
            const string asyncStateMachineAttrName = "System.Runtime.CompilerServices.AsyncStateMachineAttribute";

            return method.GetCustomAttributes(false)
                .Cast<Attribute>()
                .FirstOrDefault(attribute => attribute.GetType().FullName == asyncStateMachineAttrName);
        }

        static PropertyInfo FindAsyncStateMachineTypeProperty(Attribute stateMachineAttribute)
        {
            var typeProperty = stateMachineAttribute.GetType().GetProperty("StateMachineType");

            return typeProperty;
        }

    }
}
