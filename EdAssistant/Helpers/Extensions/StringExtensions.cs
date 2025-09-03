using System;

namespace EdAssistant.Helpers.Extensions;

public static class StringExtensions
{
    public static string Capitalize(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        return string.Create(input.Length, input, (span, value) =>
        {
            span[0] = char.ToUpper(value[0]);
            value.AsSpan(1).CopyTo(span[1..]);
        });
    }
}