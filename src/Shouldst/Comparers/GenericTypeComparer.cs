﻿using System.Reflection;

namespace Shouldst.Comparers;

internal class GenericTypeComparer<T> : ICompareStrategy<T>
{
    public ComparisionResult Compare(T x, T y)
    {
        var type = typeof(T);

        if (!type.GetTypeInfo().IsValueType || (type.GetTypeInfo().IsGenericType && type.IsNullable()))
        {
            if (x.IsEqualToDefault())
            {
                return new ComparisionResult(y.IsEqualToDefault() ? 0 : -1);
            }

            if (y.IsEqualToDefault())
            {
                return new ComparisionResult(-1);
            }
        }

        return new NoResult();
    }
}
