using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace PRA.GUI.Converters;

/// <summary>Returns true when the bound string is non-null and not just whitespace. Used to toggle validation error text.</summary>
public class StringNotEmptyConverter : IValueConverter {

    public readonly static StringNotEmptyConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return value is string s && !string.IsNullOrWhiteSpace(s);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotSupportedException();
    }

}
