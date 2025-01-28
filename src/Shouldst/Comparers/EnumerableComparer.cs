using System.Collections;

namespace Shouldst.Comparers;

internal class EnumerableComparer<T> : INullableComparer<T>
{
    public int? Compare(T x, T y)
    {
        if (x is not IEnumerable enumerableX || y is not IEnumerable enumerableY)
        {
            return null;
        }

        var enumeratorX = enumerableX.GetEnumerator();
        var enumeratorY = enumerableY.GetEnumerator();
            
        using var disposableX = enumeratorX as IDisposable;
        using var disposableY = enumeratorY as IDisposable;

        while (true)
        {
            var hasNextX = enumeratorX.MoveNext();
            var hasNextY = enumeratorY.MoveNext();

            if (!hasNextX || !hasNextY)
            {
                return hasNextX == hasNextY
                    ? 0
                    : -1;
            }

            if (!Equals(enumeratorX.Current, enumeratorY.Current))
            {
                return -1;
            }
        }
    }
}
