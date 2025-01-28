namespace Shouldst.Comparers;

internal interface INullableComparer<in T>
{
    int? Compare(T x, T y);
}
