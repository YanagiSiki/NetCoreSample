using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public static bool IsNotNull<T>(this IEnumerable<T> list)
        {
            return list != null && list.ToList().Count != 0;
        }

        public static List<T> Pagination<T>(this IQueryable<T> list, int currentPage, int pageSize = 5)where T : class
        {
            var items = list.Skip(
                    (currentPage - 1) * pageSize)
                .Take(pageSize).ToList();
            return items;
        }
    }
}