namespace Shouldst;

internal static class TypeExtensions
{
    public static bool IsNullable(this Type type)
    {
        return type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>));
    }

    public static object TryToChangeType(this object original, Type type)
    {
        try
        {
            return Convert.ChangeType(original, type);
        }
        catch
        {
            return original;
        }
    }
}
