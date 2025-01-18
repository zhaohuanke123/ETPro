using System.Collections.Generic;

namespace ET
{
    public static class CollectionHelper
    {
        public static void AddMany<T>(this List<T> list, T item, int size)
        {
            for (int i = 0; i < size; i++)
            {
                list.Add(item);
            }
        }
    }
}