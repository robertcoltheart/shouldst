﻿namespace Shouldst.Comparers;

internal class ComparableComparer<T> : ICompareStrategy<T>
{
    public ComparisionResult Compare(T x, T y)
    {
        if (x is IComparable<T> comparable1)
        {
            return new ComparisionResult(comparable1.CompareTo(y));
        }

        if (x is IComparable comparable2)
        {
            if (!(comparable2.GetType().IsInstanceOfType(y)))
            {
                return new NoResult();
            }

            return new ComparisionResult(comparable2.CompareTo(y));
        }

        return new NoResult();
    }
}
