namespace Shouldst.Comparers;

internal class EquatableComparer<T> : ICompareStrategy<T>
{
    public ComparisionResult Compare(T x, T y)
    {
        if (x is not IEquatable<T> equatable)
        {
            return new NoResult();
        }

        return new ComparisionResult(equatable.Equals(y) ? 0 : -1);
    }
}
