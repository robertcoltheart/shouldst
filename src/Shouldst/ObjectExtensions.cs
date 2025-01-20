namespace Shouldst;

internal static class ObjectExtensions
{
    public static bool IsEqualToDefault<T>(this T obj)
    {
        return Equals(obj, default(T));
    }
}
