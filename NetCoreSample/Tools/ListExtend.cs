using System;
using System.Collections.Generic;

namespace NetCoreSample.Tools
{
    public static class ListExtent
    {
        public static T GetRandomItem<T>(this List<T> list)
        {
            Random rnd = new Random();
            int r = rnd.Next(list.Count);
            return list[r];
        }
    }
}