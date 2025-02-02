namespace Shouldst;

internal static class TypeExtensions
{
    public static bool IsNullable(this Type type)
    {
        return type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>));
    }
}
