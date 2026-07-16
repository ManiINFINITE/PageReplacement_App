using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace PRA.GUI.Converters;

/// <summary>
///     Returns true when the bound value equals the ConverterParameter. Used to drive
///     Classes.Active="{Binding Navigation.CurrentPage, Converter=..., ConverterParameter=...}"
///     on the navbar buttons.
/// </summary>
public class EnumEqualsConverter : IValueConverter
{
    public readonly static EnumEqualsConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || parameter is null) return false;

        return value.Equals(parameter);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}