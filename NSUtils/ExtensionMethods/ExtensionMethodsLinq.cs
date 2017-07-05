using System;
using System.Collections.Generic;
using System.Linq;

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
        public static T FirstOrNew<T>(this IEnumerable<T> list) where T : new()
        {
            var element = list.FirstOrDefault();

            if (element != null)
            {
                return element;
            }

            return new T();
        }

        public static T FirstOrNew<T>(this IEnumerable<T> list, Func<T, bool> predicate) where T : new()
        {
            var element = list.FirstOrDefault(predicate);

            if (element != null)
            {
                return element;
            }

            return new T();
        }

        public static List<T> Clone<T>(this List<T> source)
        {
            var newList = new List<T>();

            source.ForEach(x => newList.Add(x));

            return newList;
        }
    }
}
