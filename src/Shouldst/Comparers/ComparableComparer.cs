namespace Shouldst.Comparers;

internal class ComparableComparer<T> : INullableComparer<T>
{
    public int? Compare(T x, T y)
    {
        return x switch
        {
            IComparable<T> comparable => comparable.CompareTo(y),
            IComparable comparable when comparable.GetType().IsInstanceOfType(y) => comparable.CompareTo(y),
            _ => null
        };
    }
}
