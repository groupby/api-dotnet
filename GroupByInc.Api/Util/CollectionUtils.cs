using System.Collections;
using System.Collections.Generic;

namespace GroupByInc.Api.Util
{
    public class CollectionUtils
    {
        public static bool IsNullOrEmpty(ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static bool IsNotNullOrEmpty(ICollection collection)
        {
            return !IsNullOrEmpty(collection);
        }

        public static void AddAll<T>(ICollection<T> collection, params T[] values)
        {
            if (IsNotNullOrEmpty(values))
            {
                foreach (T value in values)
                {
                    collection.Add(value);
                }   
            }
        }

        public static void AddAll<T>(ICollection<T> collection, ICollection<T> collection2)
        {
            if (collection2 == null || collection2.Count == 0)
            {
                return;
            }

            foreach (T value in collection2)
            {
                collection.Add(value);
            }
        }

        public static List<T> CollectionToList<T>(ICollection other)
        {
            if (IsNullOrEmpty(other))
            {
                return default(List<T>);
            }
            List<T> output = new List<T>(other.Count);

            IDictionaryEnumerator enumerator = (IDictionaryEnumerator) other.GetEnumerator();

            while (enumerator.MoveNext())
            {
                output.Add((T) enumerator.Value);
            }

            return output;
        }
    }
}