using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NSUtils
{
    public static class ExtensionMethodsCollections
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> elements)
        {
            foreach (var element in elements.Where(x => x != null).ToArray())
            {
                collection.Add(element);
            }
        }

        public static void RemoveCollection<T>(this ObservableCollection<T> list, IEnumerable<T> listToDeleted)
        {
            foreach (var elementToDeleted in listToDeleted)
            {
                list.Remove(elementToDeleted);
            }
        }

        public static void RemoveCollection<T>(this ObservableCollection<T> list, Func<T, bool> function)
        {
            var listToRemove = list.Where(function).ToList();
            list.RemoveCollection(listToRemove);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> invokeAction)
        {
            foreach (var element in collection)
            {
                invokeAction.Invoke(element);
            }
        }

        public static void AddRange<K, V>(this Dictionary<K, V> dictionary, Dictionary<K, V> values)
        {
            foreach (var value in values)
            {
                dictionary.Add(value.Key, value.Value);
            }
        }

        public static void Add<T>(this List<T> list, Func<T> newFunc)
        {
            list.Add(newFunc.Invoke());
        }

        public static void RemoveCollection<T>(this List<T> list, List<T> listToDeleted)
        {
            foreach (var elementToDeleted in listToDeleted)
            {
                list.Remove(elementToDeleted);
            }
        }
    }
}
