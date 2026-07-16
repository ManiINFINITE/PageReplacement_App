using PRA.Core.Interfaces;
using PRA.Core.Models;
using PRA.Core.Utilities;

namespace PRA.Core.Algorithms;

public class ClockAlgorithm : IPageReplacementAlgorithm
{
    public string Name => "Second Chance (Clock)";

    public SimulationResult Run(IReadOnlyList<int> referenceString, int frameCount)
    {
        var result = new SimulationResult
        {
            AlgorithmName = Name
        };

        if (frameCount <= 0 || referenceString.Count == 0)
            return result;

        var frames = new List<int>();
        var referenceBits = new List<bool>();
        int pointer = 0;

        foreach (int page in referenceString)
        {
            bool pageFault = false;
            int? replacedPage = null;

            int pageIndex = frames.IndexOf(page);

            if (pageIndex != -1)
            {
                // Hit
                result.PageHits++;
                referenceBits[pageIndex] = true;
            }
            else
            {
                // Fault
                pageFault = true;
                result.PageFaults++;

                if (frames.Count < frameCount)
                {
                    frames.Add(page);
                    referenceBits.Add(true);
                }
                else
                {
                    int victimIndex = FindVictim(referenceBits, ref pointer);

                    replacedPage = frames[victimIndex];
                    frames[victimIndex] = page;
                    referenceBits[victimIndex] = true;
                }
            }

            result.Steps.Add(new SimulationStep
            {
                CurrentPage = page,
                IsPageFault = pageFault,
                ReplacedPage = replacedPage,
                Frames = FrameSnapshot.CreateFrameSnapshot(frames, frameCount),
                ReferenceBits = [.. referenceBits],
                ClockPointer = pointer
            });
        }

        return result;
    }

    private static int FindVictim(List<bool> referenceBits, ref int pointer)
    {
        while (true)
        {
            if (!referenceBits[pointer])
            {
                int victimIndex = pointer;
                pointer = (pointer + 1) % referenceBits.Count;
                return victimIndex;
            }

            referenceBits[pointer] = false;
            pointer = (pointer + 1) % referenceBits.Count;
        }
    }
}