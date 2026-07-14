using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PRA.GUI.Services;

/// <summary>Parsing/validation shared by every form that asks for a reference string and frame count.</summary>
public static class InputParsing {

    public static bool TryParseReferenceString(string input, out List<int> values, out string? error) {
        values = [];
        error = null;

        var tokens = Regex.Split(input.Trim(), @"[\s,]+")
            .Where(t => t.Length > 0)
            .ToList();

        if (tokens.Count == 0) {
            error = "Enter at least one page number.";
            return false;
        }

        foreach (var token in tokens) {
            if (!int.TryParse(token, out var value)) {
                error = $"'{token}' isn't a valid page number.";
                values = [];
                return false;
            }
            values.Add(value);
        }

        return true;
    }

    public static bool TryParseFrameCount(string input, out int frameCount, out string? error) {
        error = null;

        if (!int.TryParse(input.Trim(), out frameCount) || frameCount <= 0) {
            error = "Frame count must be a positive whole number.";
            frameCount = 0;
            return false;
        }

        return true;
    }

}