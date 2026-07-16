using PRA.Core.Interfaces;
using PRA.Core.Models;
using PRA.Core.Utilities;

namespace PRA.Core.Algorithms;

public class OptimalAlgorithm : IPageReplacementAlgorithm
{
    public string Name => "Optimal";

    public SimulationResult Run(IReadOnlyList<int> referenceString, int frameCount)
    {
        var result = new SimulationResult
        {
            AlgorithmName = Name
        };

        if (frameCount <= 0 || referenceString.Count == 0) return result;

        var frames = new List<int>();

        for (int currentIndex = 0; currentIndex < referenceString.Count; currentIndex++)
        {
            bool pageFault = false;
            int? replacedPage = null;
            int page = referenceString[currentIndex];

            if (frames.Contains(page))
            {
                result.PageHits++;
            }
            else
            {
                pageFault = true;
                result.PageFaults++;

                if (frames.Count < frameCount)
                {
                    frames.Add(page);
                }
                else
                {
                    int victim = FindVictim(frames, referenceString, currentIndex);
                    replacedPage = victim;

                    int victimIndex = frames.IndexOf(victim);
                    frames[victimIndex] = page;
                }
            }

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

    private static int FindNextOccurrence(
        int page,
        IReadOnlyList<int> referenceString,
        int currentIndex
    )
    {
        for (int i = currentIndex + 1; i < referenceString.Count; i++)
        {
            if (referenceString[i] == page) return i;
        }

        return -1;
    }

    private static int FindVictim(
        List<int> frames,
        IReadOnlyList<int> referenceString,
        int currentIndex
    )
    {
        int victim = frames[0];
        int farthestUse = -1;

        foreach (int page in frames)
        {
            int nextUse = FindNextOccurrence(page, referenceString, currentIndex);

            // Never used again
            if (nextUse == -1) return page;

            if (nextUse > farthestUse)
            {
                farthestUse = nextUse;
                victim = page;
            }
        }

        return victim;
    }
}