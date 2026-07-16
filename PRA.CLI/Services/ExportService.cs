using PRA.Core.Models;
using System.Text;

namespace PRA.CLI.Services;

public static class ExportService
{
    // ---------- Single algorithm run ----------

    public static string ToCsv(SimulationResult result, IReadOnlyList<int> referenceString)
    {
        var sb = new StringBuilder();
        int frameCount = result.Steps[0].Frames.Count;

        sb.Append("Step,Page");

        for (int f = 0; f < frameCount; f++)
        {
            sb.Append($",F{f}");
        }

        sb.AppendLine(",Result,Replaced");

        for (int i = 0; i < result.Steps.Count; i++)
        {
            var step = result.Steps[i];
            sb.Append($"{i + 1},{step.CurrentPage}");
            foreach (int? frame in step.Frames) sb.Append($",{frame?.ToString() ?? ""}");
            sb.Append($",{(step.IsPageFault ? "Fault" : "Hit")}");
            sb.AppendLine($",{step.ReplacedPage?.ToString() ?? ""}");
        }

        sb.AppendLine();
        sb.AppendLine($"Algorithm,{result.AlgorithmName}");
        sb.AppendLine($"Frames,{frameCount}");
        sb.AppendLine($"Hits,{result.PageHits}");
        sb.AppendLine($"Faults,{result.PageFaults}");
        sb.AppendLine($"HitRatio,{result.HitRatio:P1}");

        return sb.ToString();
    }

    public static string ToMarkdown(SimulationResult result, IReadOnlyList<int> referenceString)
    {
        var sb = new StringBuilder();
        int frameCount = result.Steps[0].Frames.Count;

        sb.AppendLine($"# {result.AlgorithmName} — Simulation Result");
        sb.AppendLine();
        sb.AppendLine($"**Reference string:** {string.Join(' ', referenceString)}  ");
        sb.AppendLine($"**Frames:** {frameCount}  ");
        sb.AppendLine($"**Hits:** {result.PageHits}  ");
        sb.AppendLine($"**Faults:** {result.PageFaults}  ");
        sb.AppendLine($"**Hit ratio:** {result.HitRatio:P1}");
        sb.AppendLine();

        sb.Append("| Step | Page |");

        for (int f = 0; f < frameCount; f++)
        {
            sb.Append($" F{f} |");
        }

        sb.AppendLine(" Result | Replaced |");

        sb.Append("|---|---|");

        for (int f = 0; f < frameCount; f++)
        {
            sb.Append("---|");
        }

        sb.AppendLine("---|---|");

        for (int i = 0; i < result.Steps.Count; i++)
        {
            var step = result.Steps[i];
            sb.Append($"| {i + 1} | {step.CurrentPage} |");
            foreach (int? frame in step.Frames) sb.Append($" {frame?.ToString() ?? "-"} |");
            sb.Append(step.IsPageFault ? " Fault |" : " Hit |");
            sb.AppendLine($" {step.ReplacedPage?.ToString() ?? "-"} |");
        }

        return sb.ToString();
    }

    // ---------- Comparison of multiple algorithms ----------

    public static string ComparisonToCsv(IReadOnlyList<SimulationResult> results, IReadOnlyList<int> referenceString)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Algorithm,Hits,Faults,HitRatio");

        foreach (var r in results)
            sb.AppendLine($"{r.AlgorithmName},{r.PageHits},{r.PageFaults},{r.HitRatio:P1}");

        return sb.ToString();
    }

    public static string ComparisonToMarkdown(
        IReadOnlyList<SimulationResult> results,
        IReadOnlyList<int> referenceString
    )
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Algorithm Comparison");
        sb.AppendLine();
        sb.AppendLine($"**Reference string:** {string.Join(' ', referenceString)}  ");
        sb.AppendLine($"**Frames:** {results[0].Steps[0].Frames.Count}");
        sb.AppendLine();
        sb.AppendLine("| Algorithm | Hits | Faults | Hit Ratio |");
        sb.AppendLine("|---|---|---|---|");

        foreach (var r in results.OrderByDescending(r => r.HitRatio))
            sb.AppendLine($"| {r.AlgorithmName} | {r.PageHits} | {r.PageFaults} | {r.HitRatio:P1} |");

        return sb.ToString();
    }

    // ---------- IO ----------

    public static string Save(string content, string fileName)
    {
        string dir = Path.Combine(Environment.CurrentDirectory, "exports");
        Directory.CreateDirectory(dir);

        string path = Path.Combine(dir, fileName);
        File.WriteAllText(path, content);
        return path;
    }
}