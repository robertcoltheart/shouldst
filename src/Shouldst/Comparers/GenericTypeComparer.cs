namespace Shouldst.Comparers;

internal class GenericTypeComparer<T> : INullableComparer<T>
{
    public int? Compare(T x, T y)
    {
        var type = typeof(T);

        if (type.IsValueType && (!type.IsGenericType || !type.IsNullable()))
        {
            return null;
        }

        if (Equals(x, default(T)))
        {
            return Equals(y, default(T))
                ? 0
                : -1;
        }

        if (Equals(y, default(T)))
        {
            return -1;
        }

        return null;
    }
}
