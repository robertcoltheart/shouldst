using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Shouldst.Comparers;

namespace Shouldst;

public static class ShouldExtensionMethods
{
    public static void ShouldBeFalse(this bool condition)
    {
        if (condition)
        {
            throw new ShouldException("Should be [false] but is [true]");
        }
    }

    public static void ShouldBeFalse(this bool? condition)
    {
        if (condition is not { } boolCondition)
        {
            throw new ShouldException("Should be [false] but is [null]");
        }

        boolCondition.ShouldBeFalse();
    }

    public static void ShouldBeTrue(this bool condition)
    {
        if (!condition)
        {
            throw new ShouldException("Should be [true] but is [false]");
        }
    }

    public static void ShouldBeTrue(this bool? condition)
    {
        if (condition is not { } boolCondition)
        {
            throw new ShouldException("Should be [true] but is [null]");
        }

        boolCondition.ShouldBeTrue();
    }

    public static void ShouldBe<T>(this T actual, T expected)
    {
        actual.ShouldEqual(expected);
    }

    public static void ShouldNotBe<T>(this T actual, T expected)
    {
        actual.ShouldNotEqual(expected);
    }

    public static void ShouldEqual<T>(this T actual, T expected)
    {
        if (AssertComparer<T>.Default.Compare(actual, expected) != 0)
        {
            throw new ShouldException(MessageExtensions.FormatErrorMessage(actual, expected));
        }
    }

    public static void ShouldNotEqual<T>(this T actual, T expected)
    {
        if (AssertComparer<T>.Default.Compare(actual, expected) == 0)
        {
            throw new ShouldException($"Should not equal {expected.ToUsefulString()} but does: {actual.ToUsefulString()}");
        }
    }

    public static void ShouldBeNull(this object? actual)
    {
        if (actual != null)
        {
            throw new ShouldException($"Should be [null] but is {actual.ToUsefulString()}");
        }
    }

    public static void ShouldNotBeNull(this object? actual)
    {
        if (actual == null)
        {
            throw new ShouldException("Should be [not null] but is [null]");
        }
    }

    public static void ShouldBeTheSameAs(this object? actual, object? expected)
    {
        if (!ReferenceEquals(actual, expected))
        {
            throw new ShouldException($"Should be the same as {expected} but is {actual}");
        }
    }

    public static void ShouldNotBeTheSameAs(this object? actual, object? expected)
    {
        if (ReferenceEquals(actual, expected))
        {
            throw new ShouldException($"Should not be the same as {expected} but is {actual}");
        }
    }

    public static void ShouldBeOfExactType<T>(this object? actual)
    {
        actual.ShouldBeOfExactType(typeof(T));
    }

    public static void ShouldNotBeOfExactType<T>(this object? actual)
    {
        actual.ShouldNotBeOfExactType(typeof(T));
    }

    public static void ShouldBeOfExactType(this object? actual, Type expected)
    {
        if (actual == null)
        {
            throw new ShouldException($"Should be of type {expected} but is [null]");
        }

        if (actual.GetType() != expected)
        {
            throw new ShouldException($"Should be of type {expected} but is of type {actual.GetType()}");
        }
    }

    public static void ShouldNotBeOfExactType(this object? actual, Type expected)
    {
        if (actual == null)
        {
            throw new ShouldException($"Should not be of type {expected} but is [null]");
        }

        if (actual.GetType() == expected)
        {
            throw new ShouldException($"Should not be of type {expected} but is of type {actual.GetType()}");
        }
    }

    public static void ShouldBeAssignableTo<T>(this object? actual)
    {
        actual.ShouldBeAssignableTo(typeof(T));
    }

    public static void ShouldNotBeAssignableTo<T>(this object? actual)
    {
        actual.ShouldNotBeAssignableTo(typeof(T));
    }

    public static void ShouldBeAssignableTo(this object? actual, Type expected)
    {
        if (actual == null)
        {
            throw new ShouldException($"Should be assignable to type {expected} but is [null]");
        }

        if (!expected.IsInstanceOfType(actual))
        {
            throw new ShouldException($"Should be assignable to type {expected} but is not. Actual type is {actual.GetType()}");
        }
    }

    public static void ShouldNotBeAssignableTo(this object? actual, Type expected)
    {
        if (actual == null)
        {
            throw new ShouldException($"Should not be assignable to type {expected} but is [null]");
        }

        if (expected.IsInstanceOfType(actual))
        {
            throw new ShouldException($"Should not be assignable to type {expected} but is. Actual type is {actual.GetType()}");
        }
    }

    public static void ShouldMatch<T>(this T actual, Expression<Func<T, bool>> condition)
    {
        var matches = condition.Compile().Invoke(actual);

        if (!matches)
        {
            throw new ShouldException($"Should match expression [{condition}], but does not.");
        }
    }

    public static void ShouldAllBe<T>(this IEnumerable<T> actual, Expression<Func<T, bool>> condition)
    {
        var predicate = condition.Compile();

        var failing = actual
            .Where(x => predicate(x) == false)
            .ToArray();

        if (failing.Any())
        {
            throw new ShouldException(
                $"""
                 Should contain only elements conforming to: {condition}
                 the following items did not meet the condition: {failing.EachToUsefulString()}
                 """);
        }
    }

    public static void ShouldContain<T>(this IEnumerable<T> actual, params T[] expected)
    {
        actual.ShouldContain(expected.AsEnumerable());
    }

    public static void ShouldContain(this IEnumerable actual, params object[] expected)
    {
        actual.Cast<object>().ShouldContain(expected.AsEnumerable());
    }

    public static void ShouldContain<T>(this IEnumerable<T> actual, params IEnumerable<T> expected)
    {
        var actualItems = actual.ToArray();
        var expectedItems = expected.ToArray();

        var missing = expectedItems
            .Where(x => !actualItems.Contains(x, AssertComparer<T>.Default))
            .ToArray();

        if (missing.Any())
        {
            throw new ShouldException(
                $"""
                 Should contain: {expectedItems.EachToUsefulString()}
                 entire list: {actualItems.EachToUsefulString()}
                 does not contain: {missing.EachToUsefulString()}
                 """);
        }
    }

    public static void ShouldContain<T>(this IEnumerable<T> actual, Expression<Func<T, bool>> condition)
    {
        var predicate = condition.Compile();
        var actualItems = actual.ToArray();

        if (!actualItems.Any(predicate))
        {
            throw new ShouldException(
                $"""
                 Should contain elements conforming to: {condition}
                 entire list: {actualItems.EachToUsefulString()}
                 """);
        }
    }

    public static void ShouldNotContain(this IEnumerable actual, params object[] expected)
    {
        actual.Cast<object>().ShouldNotContain(expected.AsEnumerable());
    }

    public static void ShouldNotContain<T>(this IEnumerable<T> actual, params T[] expected)
    {
        actual.ShouldNotContain(expected.AsEnumerable());
    }

    public static void ShouldNotContain<T>(this IEnumerable<T> actual, params IEnumerable<T> expected)
    {
        var actualItems = actual.ToArray();
        var expectedItems = expected.ToArray();

        var contains = expectedItems
            .Where(x => actualItems.Contains(x, AssertComparer<T>.Default))
            .ToArray();

        if (contains.Any())
        {
            throw new ShouldException(
                $"""
                 Should not contain: {expectedItems.EachToUsefulString()}
                 entire list: {actualItems.EachToUsefulString()}
                 does contain: {contains.EachToUsefulString()}
                 """);
        }
    }

    public static void ShouldNotContain<T>(this IEnumerable<T> actual, Expression<Func<T, bool>> condition)
    {
        var predicate = condition.Compile();

        var actualItems = actual.ToArray();
        var contains = actualItems.Where(predicate).ToArray();

        if (contains.Any())
        {
            throw new ShouldException(
                $"""
                 No elements should conform to: {condition}
                 entire list: {actualItems.EachToUsefulString()}
                 does contain: {contains.EachToUsefulString()}
                 """);
        }
    }

    public static void ShouldBeGreaterThan<T>(this T? actual, T? expected)
        where T : IComparable<T>
    {
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (actual == null)
        {
            throw new ShouldException($"Should be greater than {expected.ToUsefulString()} but is [null]");
        }

        if (actual.CompareTo(expected) <= 0)
        {
            throw new ShouldException($"Should be greater than {expected.ToUsefulString()} but is {actual.ToUsefulString()}");
        }
    }

    public static void ShouldBeGreaterThanOrEqualTo<T>(this T? actual, T? expected)
        where T : IComparable<T>
    {
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (actual == null)
        {
            throw new ShouldException($"Should be greater than or equal to {expected.ToUsefulString()} but is [null]");
        }

        if (actual.CompareTo(expected) < 0)
        {
            throw new ShouldException($"Should be greater than or equal to {expected.ToUsefulString()} but is {actual.ToUsefulString()}");
        }
    }

    public static void ShouldBeLessThan<T>(this T? actual, T? expected)
        where T : IComparable<T>
    {
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (actual == null)
        {
            throw new ShouldException($"Should be less than {expected.ToUsefulString()} but is [null]");
        }

        if (actual.CompareTo(expected) >= 0)
        {
            throw new ShouldException($"Should be less than {expected.ToUsefulString()} but is {actual.ToUsefulString()}");
        }
    }

    public static void ShouldBeLessThanOrEqualTo<T>(this T? actual, T expected)
        where T : IComparable<T>
    {
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (actual == null)
        {
            throw new ShouldException($"Should be less than or equal to {expected.ToUsefulString()} but is [null]");
        }

        if (actual.CompareTo(expected) > 0)
        {
            throw new ShouldException($"Should be less than or equal to {expected.ToUsefulString()} but is {actual.ToUsefulString()}");
        }
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

    public static void ShouldBeEmpty(this IEnumerable actual)
    {
        var items = actual.Cast<object>().ToArray();

        if (items.Any())
        {
            throw NewException($"Should be empty but contains:{Environment.NewLine}" + items.EachToUsefulString());
        }
    }

    public static void ShouldBeEmpty(this string? actual)
    {
        if (actual == null)
        {
            throw new ShouldException("Should be empty but is [null]");
        }

        if (!string.IsNullOrEmpty(actual))
        {
            throw NewException("Should be empty but is {0}", actual);
        }
    }

    public static void ShouldNotBeEmpty(this IEnumerable actual)
    {
        if (!actual.Cast<object>().Any())
        {
            throw NewException("Should not be empty but is");
        }
    }

    public static void ShouldNotBeEmpty(this string? actual)
    {
        if (string.IsNullOrEmpty(actual))
        {
            throw NewException("Should not be empty but is");
        }
    }

    public static void ShouldMatch(this string? actual, string pattern)
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

    public static void ShouldMatch(this string? actual, Regex pattern)
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

    public static void ShouldContain(this string? actual, string expected)
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

    public static void ShouldNotContain(this string? actual, string expected)
    {
        if (expected == null)
        {
            throw new ArgumentNullException(nameof(expected));
        }

        if (actual == null)
        {
            return;
        }

        if (actual.Contains(expected))
        {
            throw NewException("Should not contain {0} but is {1}", expected, actual);
        }
    }

    public static void ShouldBeEqualIgnoringCase(this string? actual, string expected)
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
    }

    public static void ShouldStartWith(this string? actual, string expected)
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

    public static void ShouldEndWith(this string? actual, string expected)
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

    public static void ShouldBeSurroundedWith(this string? actual, string startDelimiter, string endDelimiter)
    {
        actual.ShouldStartWith(startDelimiter);
        actual.ShouldEndWith(endDelimiter);
    }

    public static void ShouldBeSurroundedWith(this string? actual, string delimiter)
    {
        actual.ShouldStartWith(delimiter);
        actual.ShouldEndWith(delimiter);
    }

    public static void ShouldContainErrorMessage(this Exception exception, string expected)
    {
        exception.Message.ShouldContain(expected);
    }

    public static void ShouldContainOnly<T>(this IEnumerable<T> actual, params T[] expected)
    {
        actual.ShouldContainOnly((IEnumerable<T>)expected);
    }

    public static void ShouldContainOnly<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
    {
        var listArray = actual.ToArray();
        var itemsArray = expected.ToArray();

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
            var message = $"""
                           Should contain only: {itemsArray.EachToUsefulString()}
                           entire list: {listArray.EachToUsefulString()}
                           """;

            if (noContain.Any())
            {
                message += $"{Environment.NewLine}does not contain: " + noContain.EachToUsefulString();
            }

            if (source.Any())
            {
                message += $"{Environment.NewLine}does contain but shouldn't: " + source.EachToUsefulString();
            }

            throw new ShouldException(message);
        }
    }

    public static void ShouldBeThrownBy(this Type exceptionType, Action action)
    {
        var exception = CatchException(action);

        ShouldNotBeNull(exception);
        ShouldBeAssignableTo(exception, exceptionType);
    }

    public static void ShouldThrow<T>(this Action action)
        where T : Exception
    {
        var exception = CatchException(action);

        ShouldNotBeNull(exception);
        ShouldBeAssignableTo<T>(exception);
    }

    public static void ShouldThrow(this Action action, Type exceptionType)
    {
        var exception = CatchException(action);

        ShouldNotBeNull(exception);
        ShouldBeAssignableTo(exception, exceptionType);
    }

    public static void ShouldBeLike(this object actual, object expected)
    {
        var exceptions = ShouldBeLikeInternal(actual, expected, string.Empty, []).ToArray();

        if (exceptions.Any())
        {
            throw NewException(exceptions.Select(e => e.Message).Aggregate(string.Empty, (r, m) => r + m + Environment.NewLine + Environment.NewLine).TrimEnd());
        }
    }

    private static IEnumerable<ShouldException> ShouldBeLikeInternal(object? obj, object? expected, string nodeName, HashSet<ReferentialEqualityTuple> visited)
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

                return [NewException($"{{0}}:{Environment.NewLine}{errorMessage}", nodeName)];
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

    private class ReferentialEqualityTuple(object? obj, object? expected)
    {
        private readonly object? obj = obj;

        private readonly object? expected = expected;

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
