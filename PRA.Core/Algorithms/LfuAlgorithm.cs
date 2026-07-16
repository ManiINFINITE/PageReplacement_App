using PRA.Core.Interfaces;
using PRA.Core.Models;
using PRA.Core.Utilities;

namespace PRA.Core.Algorithms;

public class LfuAlgorithm : IPageReplacementAlgorithm
{
    public string Name => "LFU";

    public SimulationResult Run(IReadOnlyList<int> referenceString, int frameCount)
    {
        var result = new SimulationResult
        {
            AlgorithmName = Name
        };

        if (frameCount <= 0 || referenceString.Count == 0) return result;

        var frames = new List<int>();
        var lastAccess = new Dictionary<int, int>();
        var frequencies = new Dictionary<int, int>();
        int currentStep = 1;


        foreach (var page in referenceString)
        {
            bool pageFault = false;
            int? replacedPage = null;

            if (frames.Contains(page))
            {
                result.PageHits++;
                frequencies[page]++;
            }
            else
            {
                pageFault = true;
                result.PageFaults++;

                if (frames.Count < frameCount)
                {
                    frames.Add(page);
                    frequencies[page] = 0;
                    frequencies[page]++;
                }
                else
                {
                    int victim = FindVictim(frames, frequencies, lastAccess);
                    replacedPage = victim;

                    int victimIndex = frames.IndexOf(victim);
                    frequencies.Remove(victim);
                    lastAccess.Remove(victim);
                    frames[victimIndex] = page;
                    frequencies[page] = 1;
                }
            }

            
            lastAccess[page] = currentStep;
            currentStep++;

            result.Steps.Add(new SimulationStep
            {
                CurrentPage = page,
                IsPageFault = pageFault,
                ReplacedPage = replacedPage,
                Frames = FrameSnapshot.CreateFrameSnapshot(frames, frameCount)
            });
        }

        return result;
    }

    private int FindVictim(
        List<int> frames,
        Dictionary<int, int> frequencies,
        Dictionary<int, int> lastAccess
    )
    {
        return frames
            .OrderBy(p => frequencies[p])
            .ThenBy(p => lastAccess[p])
            .First();
    }
}