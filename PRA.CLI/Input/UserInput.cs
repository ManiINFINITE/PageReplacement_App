using PRA.CLI.Components;
using PRA.Core.Utilities;

namespace PRA.CLI.Input;

public static class UserInput {

    public static List<int> ReadReferenceString() {
        while (true) {
            string input = BorderedInput.Show(
                "Reference String",
                placeholder: "e.g. 7 0 1 2 0 3 0 4 2 3 0 3 2");

            try {
                var list = ListConverter.ConvertToIntArray(input);
                if (list.Count > 0) return list;
            } catch {
                // loop and retry — BorderedInput re-shows on Enter with empty validate result otherwise
            }
        }
    }

    public static int ReadFrameCount() {
        string input = BorderedInput.Show(
            "Frame Count",
            placeholder: "e.g. 3",
            validate: s => int.TryParse(s, out int v) && v > 0,
            errorMessage: "Frame count must be a positive integer.");

        return int.Parse(input);
    }

}