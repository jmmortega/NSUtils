using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSUtils
{
    public static class ExtensionMethodsLinq
    {
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> collection, T element)
        {
            return collection.Concat(new T[] { element });
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> collection, T element)
        {
            return collection.Except(new T[] { element });
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> invokeAction)
        {
            foreach (var element in collection)
            {
                invokeAction.Invoke(element);
            }
        }

        public static void AddRange<K,V>(this Dictionary<K,V> dictionary, Dictionary<K,V> values)
        {
            foreach(var value in values)
            {
                dictionary.Add(value.Key, value.Value);
            }
        }

        
    }
}
