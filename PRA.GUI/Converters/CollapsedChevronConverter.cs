using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace PRA.GUI.Converters;

/// <summary>Returns a right chevron when collapsed, left chevron when expanded — used on the sidebar toggle button.</summary>
public class CollapsedChevronConverter : IValueConverter
{
    public readonly static CollapsedChevronConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not true ? "\uE138" : "\uE13A"; // › : ‹
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}