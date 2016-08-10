using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NSUtils
{
    public static class ExtensionMethodsReflection
    {
        public static List<Type> GetParentTypes(this Type childType)
        {
            var parentTypes = new List<Type>();
            parentTypes.Add(childType);

            if (childType.GetTypeInfo().BaseType != null)
            {
                parentTypes.AddRange(GetParentTypes(childType.GetTypeInfo().BaseType));
            }

            return parentTypes;
        }

        public static bool ContainsInterface(this Type myType, Type interfaceType)
        {
            return myType.GetTypeInfo().ImplementedInterfaces.Any(x => x == interfaceType);
        }
    }
}
