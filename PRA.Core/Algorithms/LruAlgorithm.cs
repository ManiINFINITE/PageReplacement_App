using PRA.Core.Interfaces;
using PRA.Core.Models;
using PRA.Core.Utilities;

namespace PRA.Core.Algorithms;

public class LruAlgorithm : IPageReplacementAlgorithm
{
    public string Name => "LRU";

    public SimulationResult Run(IReadOnlyList<int> referenceString, int frameCount)
    {
        var result = new SimulationResult
        {
            AlgorithmName = Name
        };

        if (frameCount <= 0 || referenceString.Count == 0) return result;

        var frames = new List<int>();
        var lastAccess = new Dictionary<int, int>();

        int currentStep = 1;

        foreach (int page in referenceString)
        {
            bool pageFault = false;
            int? replacedPage = null;

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
                    int victim = FindVictim(frames, lastAccess);
                    replacedPage = victim;
                    lastAccess.Remove(victim);
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

            lastAccess[page] = currentStep;
            currentStep++;
        }

        return result;
    }

    private int FindVictim(List<int> frames, Dictionary<int, int> lastAccess)
    {
        return frames.MinBy(p => lastAccess[p]);
    }
}