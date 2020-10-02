using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Wire.Transfer.In.Domain.SeedWorks;

namespace Wire.Transfer.In.Infrastructure.Domain
{
    public static class ExceptionRuleValidationEnumerationChecker
    {
        public static IEnumerable<T> GetEnumerableOfType<T>()
            where T : ExceptionRuleValidationEnumeration
        {
            var objects = Assembly.GetAssembly(typeof(T))
                ?.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))
                .Select(type => (T) Activator.CreateInstance(type))
                .ToList();

            objects?.Sort();

            return objects;
        }
    }
}