using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandParser
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> GetDuplicates<T, R>(this IEnumerable<T> values, Func<T, R> selector) where R : class
        {
            T[] array = values.ToArray();
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (selector(array[i]) == selector(array[j]))
                        yield return array[i];
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException();

            foreach (T item in values)
                action(item);
        }
    }
}
