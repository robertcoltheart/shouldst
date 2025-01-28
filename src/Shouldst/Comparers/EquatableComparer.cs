namespace Shouldst.Comparers;

internal class EquatableComparer<T> : INullableComparer<T>
{
    public int? Compare(T x, T y)
    {
        if (x is not IEquatable<T> equatable)
        {
            return null;
        }

        return equatable.Equals(y)
            ? 0
            : -1;
    }
}
