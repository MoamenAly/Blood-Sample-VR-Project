using System.Collections.Generic;

public static class EnumUtilits<T> where T : EnumComparable
{
    internal static T Find(List<T> list, int id)
    {

        for (int i = 0; i < list.Count; i++) {
            if (list[i].Compare(id)) { 
              return list[i];
            }
        }
        
        return default(T);
    }
}


