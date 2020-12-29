using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayApp
{
    public static class ListExtensions
    {
        public static void MoveItemAtIndexToFront<T>(this List<T> list, int index)
        {
            T item = list[index];
            list.RemoveAt(index);
            list.Insert(0, item);
        }
    }
}
