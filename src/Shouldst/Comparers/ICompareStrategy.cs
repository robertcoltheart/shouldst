namespace Shouldst.Comparers;

internal interface ICompareStrategy<in T>
{
    ComparisionResult Compare(T x, T y);
}
