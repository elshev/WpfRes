using System.Collections;
using System.Collections.Generic;

namespace Common.Collections
{
    /// <summary>
    /// Extension methods for collections a la Linq
    /// </summary>
    public static class CollectionExtensions
    {
        public static void AddRange(this IList list, IEnumerable listToAdd)
        {
            foreach (object item in listToAdd)
            {
                list.Add(item);
            }
        }

        public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> listToAdd)
        {
            foreach (T item in listToAdd)
            {
                list.Add(item);
            }
        }
    }
}
