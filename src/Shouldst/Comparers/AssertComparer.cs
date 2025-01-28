using System.Runtime.CompilerServices;

namespace Shouldst.Comparers;

internal class AssertComparer<T> : IComparer<T>, IEqualityComparer<T>
{
    public static readonly AssertComparer<T> Default = new();

    private readonly IReadOnlyCollection<INullableComparer<T>> comparers = ComparerFactory.GetComparers<T>();

    private readonly IComparer<T> defaultComparer = ComparerFactory.GetDefaultComparer<T>();

    public int Compare(T x, T y)
    {
        foreach (var comparer in comparers)
        {
            var result = comparer.Compare(x, y);

            if (result != null)
            {
                return result.Value;
            }
        }

        return defaultComparer.Compare(x, y);
    }

    public bool Equals(T x, T y)
    {
        return Compare(x, y) == 0;
    }

    public int GetHashCode(T obj)
    {
        return RuntimeHelpers.GetHashCode(obj);
    }
}
