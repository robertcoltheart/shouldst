namespace Shouldst.Comparers;

internal static class ComparerFactory
{
    public static IEnumerable<ICompareStrategy<T>> GetComparers<T>()
    {
        return
        [
            new EnumerableComparer<T>(),
            new GenericTypeComparer<T>(),
            new ComparableComparer<T>(),
            new EquatableComparer<T>(),
            new TypeComparer<T>()
        ];
    }

    public static IComparer<T> GetDefaultComparer<T>()
    {
        return new DefaultComparer<T>();
    }
}
