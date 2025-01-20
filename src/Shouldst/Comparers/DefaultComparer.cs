namespace Shouldst.Comparers;

internal class DefaultComparer<T> : IComparer<T>
{
    public int Compare(T x, T y)
    {
        return Equals(x, y)
            ? 0
            : -1;
    }
}
