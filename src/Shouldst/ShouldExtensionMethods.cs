using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Shouldst.Comparers;

namespace Shouldst;

public static class ShouldExtensionMethods
{
    public static void ShouldBeFalse(this bool? condition)
    {
        if (condition is null or true)
        {
            throw new ShouldException("Should be [false] but is [true]");
        }
    }

    public static void ShouldBeTrue(this bool? condition)
    {
        if (condition is null or false)
        {
            throw new ShouldException("Should be [true] but is [false]");
        }
    }

    public static T ShouldEqual<T>(this T actual, T expected)
    {
        if (!actual.SafeEquals(expected))
        {
            throw new ShouldException(MessageExtensions.FormatErrorMessage(actual, expected));
        }

        return actual;
    }

    public static T ShouldNotEqual<T>(this T actual, T expected)
    {
        if (actual.SafeEquals(expected))
        {
            throw new ShouldException($"Should not equal {expected.ToUsefulString()} but does: {actual.ToUsefulString()}");
        }

        return actual;
    }

    public static void ShouldBeNull(this object? actual)
    {
        if (actual != null)
        {
            throw new ShouldException($"Should be [null] but is {actual.ToUsefulString()}");
        }
    }

    public static object ShouldNotBeNull(this object? actual)
    {
        if (actual == null)
        {
            throw new ShouldException("Should be [not null] but is [null]");
        }

        return actual;
    }

    public static object ShouldBeTheSameAs(this object actual, object expected)
    {
        if (!ReferenceEquals(actual, expected))
        {
            throw new ShouldException($"Should be the same as {expected} but is {actual}");
        }

        return actual;
    }

    public static object ShouldNotBeTheSameAs(this object actual, object expected)
    {
        if (ReferenceEquals(actual, expected))
        {
            throw new ShouldException($"Should not be the same as {expected} but is {actual}");
        }

        return actual;
    }

    public static T ShouldBeOfExactType<T>(this object actual)
    {
        actual.ShouldBeOfExactType(typeof(T));

        return (T)actual;
    }

    public static object ShouldNotBeOfExactType<T>(this object actual)
    {
        actual.ShouldNotBeOfExactType(typeof(T));

        return actual;
    }

    public static object ShouldBeOfExactType(this object actual, Type expected)
    {
        if (actual == null)
        {
            throw new ShouldException($"Should be of type {expected} but is [null]");
        }

        if (actual.GetType() != expected)
        {
            throw new ShouldException($"Should be of type {expected} but is of type {actual.GetType()}");
        }

        return actual;
    }

    public static object ShouldNotBeOfExactType(this object actual, Type expected)
    {
        if (actual == null)
        {
            throw new ShouldException($"Should not be of type {expected} but is [null]");
        }

        if (actual.GetType() == expected)
        {
            throw new ShouldException($"Should not be of type {expected} but is of type {actual.GetType()}");
        }

        return actual;
    }

    public static object ShouldBeAssignableTo<T>(this object actual)
    {
        actual.ShouldBeAssignableTo(typeof(T));

        return actual;
    }

    public static object ShouldNotBeAssignableTo<T>(this object actual)
    {
        actual.ShouldNotBeAssignableTo(typeof(T));

        return actual;
    }

    public static object ShouldBeAssignableTo(this object actual, Type expected)
    {
        if (actual == null)
        {
            throw new ShouldException($"Should be assignable to type {expected} but is [null]");
        }

        if (!expected.IsInstanceOfType(actual))
        {
            throw new ShouldException($"Should be assignable to type {expected} but is not. Actual type is {actual.GetType()}");
        }

        return actual;
    }

    public static object ShouldNotBeAssignableTo(this object actual, Type expected)
    {
        if (actual == null)
        {
            throw new ShouldException($"Should not be assignable to type {expected} but is [null]");
        }

        if (expected.IsInstanceOfType(actual))
        {
            throw new ShouldException($"Should not be assignable to type {expected} but is. Actual type is {actual.GetType()}");
        }

        return actual;
    }

    public static T ShouldMatch<T>(this T actual, Expression<Func<T, bool>> condition)
    {
        var matches = condition.Compile().Invoke(actual);

        if (matches)
        {
            return actual;
        }

        throw new ShouldException($"Should match expression [{condition}], but does not.");
    }

    public static void ShouldEachConformTo<T>(this IEnumerable<T> list, Expression<Func<T, bool>> condition)
    {
        var source = new List<T>(list);
        var func = condition.Compile();

        var failingItems = source
            .Where(x => func(x) == false)
            .ToArray();

        if (failingItems.Any())
        {
            throw new ShouldException(string.Format("Should contain only elements conforming to: {0}" + Environment.NewLine + "the following items did not meet the condition: {1}",
                condition,
                failingItems.EachToUsefulString()));
        }
    }

    public static void ShouldContain<T>(this IEnumerable<T> list, params T[] items)
    {
        list.ShouldContain((IEnumerable<T>)items);
    }

    public static void ShouldContain(this IEnumerable list, params object[] items)
    {
        var actualList = list.Cast<object>();
        var expectedList = items.Cast<object>();

        actualList.ShouldContain(expectedList);
    }

    public static void ShouldContain<T>(this IEnumerable<T> list, IEnumerable<T> items)
    {
        var comparer = new AssertComparer<T>();

        var listArray = list.ToArray();
        var itemsArray = items.ToArray();

        var noContain = itemsArray
            .Where(x => !listArray.Contains(x, comparer))
            .ToList();

        if (noContain.Any())
        {
            throw new ShouldException(string.Format(
                "Should contain: {0}" + Environment.NewLine +
                "entire list: {1}" + Environment.NewLine +
                "does not contain: {2}",
                itemsArray.EachToUsefulString(),
                listArray.EachToUsefulString(),
                noContain.EachToUsefulString()));
        }
    }

    public static void ShouldContain<T>(this IEnumerable<T> list, Expression<Func<T, bool>> condition)
    {
        var func = condition.Compile();
        var listArray = list.ToArray();

        if (!listArray.Any(func))
        {
            throw new ShouldException(string.Format(
                "Should contain elements conforming to: {0}" + Environment.NewLine +
                "entire list: {1}",
                condition,
                listArray.EachToUsefulString()));
        }
    }

    public static void ShouldNotContain(this IEnumerable list, params object[] items)
    {
        var actualList = list.Cast<object>();
        var expectedList = items.Cast<object>();

        actualList.ShouldNotContain(expectedList);
    }

    public static void ShouldNotContain<T>(this IEnumerable<T> list, params T[] items)
    {
        list.ShouldNotContain((IEnumerable<T>)items);
    }

    public static void ShouldNotContain<T>(this IEnumerable<T> list, IEnumerable<T> items)
    {
        var comparer = new AssertComparer<T>();

        var listArray = list.ToArray();
        var itemsArray = items.ToArray();

        var contains = itemsArray
            .Where(x => listArray.Contains(x, comparer))
            .ToList();

        if (contains.Any())
        {
            throw new ShouldException(string.Format(
                "Should not contain: {0}" + Environment.NewLine +
                "entire list: {1}" + Environment.NewLine +
                "does contain: {2}",
                itemsArray.EachToUsefulString(),
                listArray.EachToUsefulString(),
                contains.EachToUsefulString()));
        }
    }

    public static void ShouldNotContain<T>(this IEnumerable<T> list, Expression<Func<T, bool>> condition)
    {
        var func = condition.Compile();

        var listArray = list.ToArray();
        var contains = listArray.Where(func).ToArray();

        if (contains.Any())
        {
            throw new ShouldException(string.Format(
                "No elements should conform to: {0}" + Environment.NewLine +
                "entire list: {1}" + Environment.NewLine +
                "does contain: {2}",
                condition,
                listArray.EachToUsefulString(),
                contains.EachToUsefulString()));
        }
    }

    public static IComparable ShouldBeGreaterThan(this IComparable arg1, IComparable arg2)
    {
        if (arg2 == null)
        {
            throw new ArgumentNullException(nameof(arg2));
        }

        if (arg1 == null)
        {
            throw NewException("Should be greater than {0} but is [null]", arg2);
        }

        if (arg1.CompareTo(arg2.TryToChangeType(arg1.GetType())) <= 0)
        {
            throw NewException("Should be greater than {0} but is {1}", arg2, arg1);
        }

        return arg1;
    }

    public static IComparable ShouldBeGreaterThanOrEqualTo(this IComparable arg1, IComparable arg2)
    {
        if (arg2 == null)
        {
            throw new ArgumentNullException(nameof(arg2));
        }

        if (arg1 == null)
        {
            throw NewException("Should be greater than or equal to {0} but is [null]", arg2);
        }

        if (arg1.CompareTo(arg2.TryToChangeType(arg1.GetType())) < 0)
        {
            throw NewException("Should be greater than or equal to {0} but is {1}", arg2, arg1);
        }

        return arg1;
    }

    public static IComparable ShouldBeLessThan(this IComparable arg1, IComparable arg2)
    {
        if (arg2 == null)
        {
            throw new ArgumentNullException(nameof(arg2));
        }

        if (arg1 == null)
        {
            throw NewException("Should be less than {0} but is [null]", arg2);
        }

        if (arg1.CompareTo(arg2.TryToChangeType(arg1.GetType())) >= 0)
        {
            throw NewException("Should be less than {0} but is {1}", arg2, arg1);
        }

        return arg1;
    }

    public static IComparable ShouldBeLessThanOrEqualTo(this IComparable arg1, IComparable arg2)
    {
        if (arg2 == null)
        {
            throw new ArgumentNullException(nameof(arg2));
        }

        if (arg1 == null)
        {
            throw NewException("Should be less than or equal to {0} but is [null]", arg2);
        }

        if (arg1.CompareTo(arg2.TryToChangeType(arg1.GetType())) > 0)
        {
            throw NewException("Should be less than or equal to {0} but is {1}", arg2, arg1);
        }

        return arg1;
    }

    public static void ShouldBeCloseTo(this float actual, float expected)
    {
        ShouldBeCloseTo(actual, expected, 0.0000001f);
    }

    public static void ShouldBeCloseTo(this float actual, float expected, float tolerance)
    {
        if (Math.Abs(actual - expected) > tolerance)
        {
            throw new ShouldException($"Should be within {tolerance.ToUsefulString()} of {expected.ToUsefulString()} but is {actual.ToUsefulString()}");
        }
    }

    public static void ShouldBeCloseTo(this double actual, double expected)
    {
        ShouldBeCloseTo(actual, expected, 0.0000001f);
    }

    public static void ShouldBeCloseTo(this double actual, double expected, double tolerance)
    {
        if (Math.Abs(actual - expected) > tolerance)
        {
            throw new ShouldException($"Should be within {tolerance.ToUsefulString()} of {expected.ToUsefulString()} but is {actual.ToUsefulString()}");
        }
    }

    public static void ShouldBeCloseTo(this decimal actual, decimal expected)
    {
        ShouldBeCloseTo(actual, expected, 0.0000001m);
    }

    public static void ShouldBeCloseTo(this decimal actual, decimal expected, decimal tolerance)
    {
        if (Math.Abs(actual - expected) > tolerance)
        {
            throw new ShouldException($"Should be within {tolerance.ToUsefulString()} of {expected.ToUsefulString()} but is {actual.ToUsefulString()}");
        }
    }

    public static void ShouldBeCloseTo(this TimeSpan actual, TimeSpan expected, TimeSpan tolerance)
    {
        if (Math.Abs(actual.Ticks - expected.Ticks) > tolerance.Ticks)
        {
            throw new ShouldException($"Should be within {tolerance.ToUsefulString()} of {expected.ToUsefulString()} but is {actual.ToUsefulString()}");
        }
    }

    public static void ShouldBeCloseTo(this DateTime actual, DateTime expected, TimeSpan tolerance)
    {
        var difference = expected - actual;

        if (Math.Abs(difference.Ticks) > tolerance.Ticks)
        {
            throw new ShouldException($"Should be within {tolerance.ToUsefulString()} of {expected.ToUsefulString()} but is {actual.ToUsefulString()}");
        }
    }

    public static void ShouldBeEmpty(this IEnumerable collection)
    {
        var items = collection.Cast<object>().ToArray();

        if (items.Any())
        {
            throw NewException("Should be empty but contains:\n" + items.EachToUsefulString());
        }
    }

    public static void ShouldBeEmpty(this string aString)
    {
        if (aString == null)
        {
            throw new ShouldException("Should be empty but is [null]");
        }

        if (!string.IsNullOrEmpty(aString))
        {
            throw NewException("Should be empty but is {0}", aString);
        }
    }

    public static void ShouldNotBeEmpty(this IEnumerable collection)
    {
        if (!collection.Cast<object>().Any())
        {
            throw NewException("Should not be empty but is");
        }
    }

    public static void ShouldNotBeEmpty(this string aString)
    {
        if (string.IsNullOrEmpty(aString))
        {
            throw NewException("Should not be empty but is");
        }
    }

    public static void ShouldMatch(this string actual, string pattern)
    {
        if (pattern == null)
        {
            throw new ArgumentNullException(nameof(pattern));
        }

        if (actual == null)
        {
            throw NewException("Should match regex {0} but is [null]", pattern);
        }

        ShouldMatch(actual, new Regex(pattern));
    }

    public static void ShouldMatch(this string actual, Regex pattern)
    {
        if (pattern == null)
        {
            throw new ArgumentNullException(nameof(pattern));
        }

        if (actual == null)
        {
            throw NewException("Should match regex {0} but is [null]", pattern);
        }

        if (!pattern.IsMatch(actual))
        {
            throw NewException("Should match {0} but is {1}", pattern, actual);
        }
    }

    public static void ShouldContain(this string actual, string expected)
    {
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (actual == null)
        {
            throw NewException("Should contain {0} but is [null]", expected);
        }

        if (!actual.Contains(expected))
        {
            throw NewException("Should contain {0} but is {1}", expected, actual);
        }
    }

    public static void ShouldNotContain(this string? actual, string notExpected)
    {
        if (notExpected == null)
        {
            throw new ArgumentNullException(nameof(notExpected));
        }

        if (actual == null)
        {
            return;
        }

        if (actual.Contains(notExpected))
        {
            throw NewException("Should not contain {0} but is {1}", notExpected, actual);
        }
    }

    public static string ShouldBeEqualIgnoringCase(this string actual, string expected)
    {
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (actual == null)
        {
            throw NewException("Should be equal ignoring case to {0} but is [null]", expected);
        }

        if (CultureInfo.InvariantCulture.CompareInfo.Compare(actual, expected, CompareOptions.IgnoreCase) != 0)
        {
            throw NewException("Should be equal ignoring case to {0} but is {1}", expected, actual);
        }

        return actual;
    }

    public static void ShouldStartWith(this string actual, string expected)
    {
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (actual == null)
        {
            throw NewException("Should start with {0} but is [null]", expected);
        }

        if (!actual.StartsWith(expected))
        {
            throw NewException("Should start with {0} but is {1}", expected, actual);
        }
    }

    public static void ShouldEndWith(this string actual, string expected)
    {
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (actual == null)
        {
            throw NewException("Should end with {0} but is [null]", expected);
        }

        if (!actual.EndsWith(expected))
        {
            throw NewException("Should end with {0} but is {1}", expected, actual);
        }
    }

    public static void ShouldBeSurroundedWith(this string actual, string expectedStartDelimiter, string expectedEndDelimiter)
    {
        actual.ShouldStartWith(expectedStartDelimiter);
        actual.ShouldEndWith(expectedEndDelimiter);
    }

    public static void ShouldBeSurroundedWith(this string actual, string expectedDelimiter)
    {
        actual.ShouldStartWith(expectedDelimiter);
        actual.ShouldEndWith(expectedDelimiter);
    }

    public static void ShouldContainErrorMessage(this Exception exception, string expected)
    {
        exception.Message.ShouldContain(expected);
    }

    public static void ShouldContainOnly<T>(this IEnumerable<T> list, params T[] items)
    {
        list.ShouldContainOnly((IEnumerable<T>)items);
    }

    public static void ShouldContainOnly<T>(this IEnumerable<T> list, IEnumerable<T> items)
    {
        var listArray = list.ToArray();
        var itemsArray = items.ToArray();

        var source = new List<T>(listArray);
        var noContain = new List<T>();
        var comparer = new AssertComparer<T>();

        foreach (var item in itemsArray)
        {
            if (!source.Contains(item, comparer))
            {
                noContain.Add(item);
            }
            else
            {
                source.Remove(item);
            }
        }

        if (noContain.Any() || source.Any())
        {
            var message = string.Format("Should contain only: {0}" + Environment.NewLine + "entire list: {1}",
                itemsArray.EachToUsefulString(),
                listArray.EachToUsefulString());

            if (noContain.Any())
            {
                message += "\ndoes not contain: " + noContain.EachToUsefulString();
            }

            if (source.Any())
            {
                message += "\ndoes contain but shouldn't: " + source.EachToUsefulString();
            }

            throw new ShouldException(message);
        }
    }

    public static Exception ShouldBeThrownBy(this Type exceptionType, Action method)
    {
        var exception = CatchException(method);

        ShouldNotBeNull(exception);
        ShouldBeAssignableTo(exception, exceptionType);

        return exception;
    }

    public static void ShouldBeLike(this object obj, object expected)
    {
        var exceptions = ShouldBeLikeInternal(obj, expected, string.Empty, []).ToArray();

        if (exceptions.Any())
        {
            throw NewException(exceptions.Select(e => e.Message).Aggregate(string.Empty, (r, m) => r + m + Environment.NewLine + Environment.NewLine).TrimEnd());
        }
    }

    private static IEnumerable<ShouldException> ShouldBeLikeInternal(object obj, object expected, string nodeName, HashSet<ReferentialEqualityTuple> visited)
    {
        var objExpectedTuple = new ReferentialEqualityTuple(obj, expected);

        if (!visited.Add(objExpectedTuple))
        {
            return [];
        }

        ObjectGraphComparer.INode? expectedNode = null;

        var nodeType = typeof(ObjectGraphComparer.LiteralNode);

        if (obj != null && expected != null)
        {
            expectedNode = ObjectGraphComparer.GetGraph(expected);
            nodeType = expectedNode.GetType();
        }

        if (nodeType == typeof(ObjectGraphComparer.LiteralNode))
        {
            try
            {
                obj.ShouldEqual(expected);
            }
            catch (ShouldException ex)
            {
                return [NewException($"{{0}}:{Environment.NewLine}{ex.Message}", nodeName)];
            }

            return [];
        }

        if (nodeType == typeof(ObjectGraphComparer.SequenceNode))
        {
            if (obj == null)
            {
                var errorMessage = MessageExtensions.FormatErrorMessage(null, expected);

                return [NewException($"{{0}}:{Environment.NewLine}{errorMessage}", nodeName)];
            }

            var actualNode = ObjectGraphComparer.GetGraph(obj);

            if (actualNode.GetType() != typeof(ObjectGraphComparer.SequenceNode))
            {
                var errorMessage = $"  Expected: Array or Sequence{Environment.NewLine}  But was:  {obj.GetType()}";

                return [NewException($"{{0}}:{Environment.NewLine}{errorMessage}", nodeName)];
            }

            var expectedValues = ((ObjectGraphComparer.SequenceNode)expectedNode)?.ValueGetters.ToArray();
            var actualValues = ((ObjectGraphComparer.SequenceNode)actualNode).ValueGetters.ToArray();

            var expectedCount = expectedValues?.Length ?? 0;
            var actualCount = actualValues.Length;

            if (expectedCount != actualCount)
            {
                var errorMessage = string.Format("  Expected: Sequence length of {1}{0}  But was:  {2}", Environment.NewLine, expectedCount, actualCount);

                return [NewException($"{{0}}:{Environment.NewLine}{errorMessage}", nodeName)];
            }

            return Enumerable.Range(0, expectedCount)
                .SelectMany(i => ShouldBeLikeInternal(actualValues.ElementAt(i)(), expectedValues?.ElementAt(i)(), $"{nodeName}[{i}]", visited));
        }

        if (nodeType == typeof(ObjectGraphComparer.KeyValueNode))
        {
            var actualNode = ObjectGraphComparer.GetGraph(obj);

            if (actualNode.GetType() != typeof(ObjectGraphComparer.KeyValueNode))
            {
                var errorMessage = $"  Expected: Class{Environment.NewLine}  But was:  {obj?.GetType()}";

                return new[] { NewException($"{{0}}:{Environment.NewLine}{errorMessage}", nodeName) };
            }

            var expectedKeyValues = ((ObjectGraphComparer.KeyValueNode)expectedNode)?.KeyValues;
            var actualKeyValues = ((ObjectGraphComparer.KeyValueNode)actualNode).KeyValues;

            return expectedKeyValues?.SelectMany(kv =>
            {
                var fullNodeName = string.IsNullOrEmpty(nodeName)
                    ? kv.Name
                    : $"{nodeName}.{kv.Name}";
                var actualKeyValue = actualKeyValues.SingleOrDefault(k => k.Name == kv.Name);

                if (actualKeyValue == null)
                {
                    var errorMessage = string.Format("  Expected: {1}{0}  But was:  Not Defined",
                        Environment.NewLine, kv.ValueGetter().ToUsefulString());

                    return [NewException($"{{0}}:{Environment.NewLine}{errorMessage}", fullNodeName)];
                }

                return ShouldBeLikeInternal(actualKeyValue.ValueGetter(), kv.ValueGetter(), fullNodeName, visited);
            }) ?? [];
        }

        throw new InvalidOperationException("Unknown node type");
    }

    private static bool SafeEquals<T>(this T left, T right)
    {
        var comparer = new AssertComparer<T>();

        return comparer.Compare(left, right) == 0;
    }

    private static ShouldException NewException(string message, params object[] parameters)
    {
        if (parameters.Any())
        {
            return new ShouldException(string.Format(message.EnsureSafeFormat(), parameters.Select(x => x.ToUsefulString()).Cast<object>().ToArray()));
        }

        return new ShouldException(message);
    }

    private static Exception? CatchException(Action throwingAction)
    {
        try
        {
            throwingAction();
        }
        catch (Exception ex)
        {
            return ex;
        }

        return null;
    }

    private class ReferentialEqualityTuple(object obj, object expected)
    {
        private readonly object obj = obj;

        private readonly object expected = expected;

        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(obj) * RuntimeHelpers.GetHashCode(expected);
        }

        public override bool Equals(object? other)
        {
            if (other is not ReferentialEqualityTuple otherSimpleTuple)
            {
                return false;
            }

            return ReferenceEquals(obj, otherSimpleTuple.obj) && ReferenceEquals(expected, otherSimpleTuple.expected);
        }
    }
}
