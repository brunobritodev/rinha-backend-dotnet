using static System.String;

namespace RinhaBackend.Utils;

public static class StringExtensions
{
    public static bool IsMissing(this string value)
    {
        return IsNullOrEmpty(value);
    }

    public static bool IsPresent(this string value)
    {
        return !IsNullOrWhiteSpace(value);
    }
}
