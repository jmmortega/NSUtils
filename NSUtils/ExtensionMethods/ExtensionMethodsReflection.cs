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

        public static T MemberwiseClone<T>(this T obj) where T : new()
        {
            var method = obj.GetType().GetRuntimeMethods().FirstOrDefault(x => x.Name == "MemberwiseClone");

            if(method != null)
            {
                return (T)method.Invoke(obj, new object[] { });
            }

            return default(T);
        }

        public static void SetValueSecure(this PropertyInfo propertyInfo , object value, object objectToSet)
        {
            try
            {
                if (value.GetType() == typeof(string))
                {
                    propertyInfo.SetValue(objectToSet, string.Empty);
                }
                else
                {
                    propertyInfo.SetValue(objectToSet, value);
                }
            }
            catch(Exception e)
            {
                throw e;
            }

        }
    }
}
