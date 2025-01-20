namespace Shouldst.Comparers;

internal class TypeComparer<T> : ICompareStrategy<T>
{
    public ComparisionResult Compare(T x, T y)
    {
        if (x.GetType() != y.GetType())
        {
            return new ComparisionResult(-1);
        }

        return new NoResult();
    }
}
