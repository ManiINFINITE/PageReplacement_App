namespace PRA.Core.Utilities;

public class ListConverter {

    public static List<int> ConvertToIntArray(ReadOnlySpan<char> input) {
        var list = new List<int>();
        var span = input.Trim();

        while (true) {
            var index = span.IndexOf(' ');

            if (index == -1) {
                if (!span.IsEmpty) list.Add(int.Parse(span));
                break;
            }

            list.Add(int.Parse(span[..index]));
            span = span[(index + 1)..];
        }

        return list;
    }

}