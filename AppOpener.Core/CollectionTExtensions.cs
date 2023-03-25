using System;
using System.Collections.Generic;
using System.Linq;


namespace AppOpener.Core
{
    public static class CollectionTExtensions
    {
        public static void AddRange<T>(this ICollection<T> instance, IEnumerable<T> collection)
        {
            Guard.IsNotNull(instance, "instance");
            Guard.IsNotNull(collection, "collection");

            foreach (T local in collection)
                instance.Add(local);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            Guard.IsNotNull(collection, "collection");

            foreach (T local in collection)
                action(local);
        }

        public static List<T> ToList<T>(this string collection, string delimiter)
        {
            return collection
                .Split(new string[] { delimiter }, StringSplitOptions.RemoveEmptyEntries)
                .Select(value => CommonHelper.To<T>(value))
                .ToList();
        }

        public static List<T> ToList<T>(this string collection)
        {
            return collection.ToList<T>(",");
        }

        public static string ToDelimitedString<T>(this IEnumerable<T> collection)
        {
            return collection.ToDelimitedString(",");
        }

        public static string ToDelimitedString<T>(this IEnumerable<T> collection, string delimiter)
        {
            return string.Join(delimiter, collection.Select(x => x.ToString()).ToArray());
        }

       

        public static void AfterListChanges<T>(this ICollection<T> list) where T : class
        {
            // ...
        }

        public static bool BeforeAddItem<T>(this ICollection<T> list, T item, Action<T> setParent) where T : class
        {
            // ...
            setParent(item);
            if (list.Any(item.Equals))
            {
                return false;
            }
            return true;
        }

        public static bool BeforeRemoveItem<T>(this ICollection<T> list, T item, Action<T> setParentToNull) where T : class
        {
            setParentToNull(item);
            if (list.Any(item.Equals))
            {
                return true;
            }
            return false;
        }

        public static IEnumerable<T> Heads<T>(this IEnumerable<IEnumerable<T>> xss)
        {
            if (xss.Any(xs => xs.IsEmpty()))
                return new List<T>();
            return xss.Select(xs => xs.First());
        }

        public static bool IsEmpty<T>(this IEnumerable<T> xs)
        {
            return xs.Count() == 0;
        }

        public static IEnumerable<IEnumerable<T>> Tails<T>(this IEnumerable<IEnumerable<T>> xss)
        {
            return xss.Select(xs => xs.Skip(1));
        }

        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> xss)
        {
            //      xss.Dump("xss in Transpose");
            var heads = xss.Heads()
                //          .Dump("heads in Transpose")
                ;
            var tails = xss.Tails()
                //          .Dump("tails in Transpose")
                ;

            var empt = new List<IEnumerable<T>>();
            if (heads.IsEmpty())
                return empt;
            empt.Add(heads);
            return empt.Concat(tails.Transpose())
                //          .Dump("empt")
                ;
        }
        #region Utilities

        #endregion
    }

    public class Group<T, K>
    {
        public K Key;
        public IEnumerable<T> Values;
    }
}
