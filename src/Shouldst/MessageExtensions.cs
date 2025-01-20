using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Shouldst;

internal static class MessageExtensions
{
    private const string CurlyBraceSurround = "{{{0}}}";

    private static readonly Regex EscapeNonFormatBraces = new(@"{([^\d].*?)}", RegexOptions.Compiled | RegexOptions.Singleline);

    internal static string EnsureSafeFormat(this string message)
    {
        return EscapeNonFormatBraces.Replace(message, match => string.Format(CurlyBraceSurround, match.Groups[0]));
    }

    internal static string ToUsefulString(this object? value)
    {
        if (value == null)
        {
            return "[null]";
        }

        if (value is string stringValue)
        {
            return $"\"{stringValue.Replace("\n", "\\n")}\"";
        }

        if (value.GetType().GetTypeInfo().IsValueType)
        {
            return $"[{value}]";
        }

        if (value is IEnumerable items)
        {
            var enumerable = items.Cast<object>();

            return $"{items.GetType()}:{Environment.NewLine}{enumerable.EachToUsefulString()}";
        }

        var message = value.ToString();

        if (message == null || message.Trim() == string.Empty)
        {
            return $"{value.GetType()}:[]";
        }

        message = message.Trim();

        if (message.Contains("\n"))
        {
            return string.Format("""
                                 {1}:
                                 [
                                 {0}
                                 ]
                                 """, message.Tab(), value.GetType());
        }

        if (value.GetType().ToString() == message)
        {
            return value.GetType().ToString();
        }

        return $"{value.GetType()}:[{message}]";
    }

    public static string EachToUsefulString<T>(this IEnumerable<T> enumerable)
    {
        var array = enumerable.ToArray();

        var message = new StringBuilder();
        message.AppendLine("{");
        message.Append(string.Join($",{Environment.NewLine}", array.Select(x => x.ToUsefulString().Tab()).Take(10).ToArray()));

        if (array.Length > 10)
        {
            if (array.Length > 11)
            {
                message.AppendLine($",{Environment.NewLine}  ...({array.Length - 10} more elements)");
            }
            else
            {
                message.AppendLine($",{Environment.NewLine}" + array.Last().ToUsefulString().Tab());
            }
        }
        else
        {
            message.AppendLine();
        }

        message.AppendLine("}");

        return message.ToString();
    }

    public static string FormatErrorMessage(object? actualObject, object? expectedObject)
    {
        if (actualObject is not string actualString || expectedObject is not string expectedString)
        {
            var actual = actualObject.ToUsefulString();
            var expected = expectedObject.ToUsefulString();

            return string.Format("  Expected: {1}{0}  But was:  {2}", Environment.NewLine, expected, actual);
        }
        else
        {
            var expectedStringLengthMessage = GetExpectedStringLengthMessage(expectedString.Length, actualString.Length);
            var firstIndexOfFirstDifference = GetFirstIndexOfFirstDifference(actualString, expectedString);

            GetStringsAroundFirstDifference(expectedString, actualString, firstIndexOfFirstDifference, out var actualReported, out var expectedReported);

            var count = GetFirstIndexOfFirstDifference(actualReported, expectedReported);

            return string.Format(
                "  {1} Strings differ at index {2}.{0}" +
                "  Expected: \"{3}\"{0}" +
                "  But was:  \"{4}\"{0}" +
                "  -----------{5}^",
                Environment.NewLine,
                expectedStringLengthMessage,
                firstIndexOfFirstDifference,
                expectedReported,
                actualReported,
                new string('-', count));
        }
    }

    private static string Tab(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var split = value.Split(["\r\n", "\n"], StringSplitOptions.None);

        var message = new StringBuilder();
        message.Append($"  {split[0]}");

        foreach (var part in split.Skip(1))
        {
            message.AppendLine();
            message.Append($"  {part}");
        }

        return message.ToString();
    }

    private static string GetExpectedStringLengthMessage(int actual, int expected)
    {
        return actual == expected
            ? $"String lengths are both {actual}."
            : $"Expected string length {actual} but was {expected}.";
    }

    private static int GetFirstIndexOfFirstDifference(string actual, string expected)
    {
        for (var i = 0; i < actual.Length; i++)
        {
            if (expected.Length <= i || expected[i] != actual[i])
            {
                return i;
            }
        }

        return actual.Length;
    }

    private static bool IsInCopyFrameLength(int start, int end, int max)
    {
        var length = end - start;

        if (start > 0)
        {
            length += 3;
        }

        if (end < max)
        {
            length += 3;
        }

        return length < 64;
    }

    private static void GetStringsAroundFirstDifference(string expected, string actual, int firstIndexOfFirstDifference, out string actualReported, out string expectedReported)
    {
        var left = firstIndexOfFirstDifference;
        var actualRight = firstIndexOfFirstDifference;
        var expectedRight = firstIndexOfFirstDifference;
        var keepAugmenting = true;

        while (keepAugmenting &&
               IsInCopyFrameLength(left, actualRight, actual.Length) &&
               IsInCopyFrameLength(left, expectedRight, expected.Length))
        {
            keepAugmenting = false;

            if (left > 0)
            {
                left--;
                keepAugmenting = true;
            }

            if (IsInCopyFrameLength(left, actualRight, actual.Length) &&
                IsInCopyFrameLength(left, expectedRight, expected.Length))
            {
                if (actual.Length > actualRight)
                {
                    actualRight++;
                    keepAugmenting = true;
                }

                if (expected.Length > expectedRight)
                {
                    expectedRight++;
                    keepAugmenting = true;
                }
            }
        }

        actualReported = actual.Substring(left, actualRight - left);
        expectedReported = expected.Substring(left, expectedRight - left);

        if (left != 0)
        {
            actualReported = "..." + actualReported;
            expectedReported = "..." + expectedReported;
        }

        if (actualRight != actual.Length || expectedRight != expected.Length)
        {
            actualReported = actualReported + "...";
            expectedReported = expectedReported + "...";
        }
    }
}
