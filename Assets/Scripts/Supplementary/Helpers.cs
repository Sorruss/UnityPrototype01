using System.Collections.Generic;

namespace FG
{
    public static class Helpers
    {
        public static void CleanUpListFromNULLs<T>(ref List<T> list)
        {
            for (int i = list.Count - 1; i >= 0; --i)
            {
                if (list[i] != null)
                    continue;

                list.RemoveAt(i);
            }
        }
    }
}