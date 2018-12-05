using System;
using System.Collections.Generic;

namespace NetCoreSample.Tools
{
    public static class ListExtend
    {
        public static T GetRandomItem<T>(this List<T> list)
        {
            Random rnd = new Random();
            int r = rnd.Next(list.Count);
            return list[r];
        }

        public static bool IsNotNull<T>(this IEnumerable<T> list){
            if(list != null) return true;
            return false;
        }
    }
}