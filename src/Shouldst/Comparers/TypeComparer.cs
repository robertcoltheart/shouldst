namespace Shouldst.Comparers;

internal class TypeComparer<T> : INullableComparer<T>
{
    public int? Compare(T x, T y)
    {
        if (x == null && y == null)
        {
            return 0;
        }

        if (x == null || y == null)
        {
            return -1;
        }

        if (x.GetType() != y.GetType())
        {
            return -1;
        }

        return null;
    }
}
