using System.Collections.Generic;

public static class TryGetValueForList
{
    public static bool IsIndexOutOfRange<T>(this T[] array, int index)
    {
        return array == null || index < 0 || index >= array.Length;
    }
    public static bool IsIndexOutOfRange<T>(this List<T> list, int index)
    {
        return list == null || index < 0 || index >= list.Count;
    }
    public static bool IsIndexOutOfRangeOrNull<T>(this T[] array, int index)
    {
        return array == null || index < 0 || index >= array.Length || array[index] == null;
    }
    public static bool IsIndexOutOfRangeOrNull<T>(this List<T> list, int index)
    {
        return list == null || index < 0 || index >= list.Count || list[index] == null;
    }

    public static bool TryGetValue<T>(this T[] array, int index, out T value)
    {
        if (array.IsIndexOutOfRangeOrNull(index))
        {
            value = default;
            return false;
        }
        value = array[index];
        return true;
    }
    public static bool TryGetValue<T>(this List<T> list, int index, out T value)
    {
        if (list.IsIndexOutOfRangeOrNull(index))
        {
            value = default;
            return false;
        }
        value = list[index];
        return true;
    }
}