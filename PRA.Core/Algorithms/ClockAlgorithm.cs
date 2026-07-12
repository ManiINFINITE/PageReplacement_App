using PRA.Core.Interfaces;
using PRA.Core.Models;
using PRA.Core.Utilities;

namespace PRA.Core.Algorithms;

public class ClockAlgorithm : IPageReplacementAlgorithm {

    public string Name => "Second Chance (Clock)";

    public SimulationResult Run(IReadOnlyList<int> referenceString, int frameCount) {
        var result = new SimulationResult {
            AlgorithmName = Name
        };

        if (frameCount <= 0 || referenceString.Count == 0)
            return result;

        var frames = new List<int>();
        var referenceBits = new List<bool>();
        int pointer = 0;

        foreach (var page in referenceString) {
            bool pageFault = false;
            int? replacedPage = null;

            int pageIndex = frames.IndexOf(page);

            if (pageIndex != -1) {
                // Hit
                result.PageHits++;
                referenceBits[pageIndex] = true;
            } else {
                // Fault
                pageFault = true;
                result.PageFaults++;

                if (frames.Count < frameCount) {
                    // Empty frame available
                    frames.Add(page);
                    referenceBits.Add(true);
                } else {
                    // Second Chance replacement
                    while (true) {
                        if (!referenceBits[pointer]) {
                            replacedPage = frames[pointer];
                            frames[pointer] = page;
                            referenceBits[pointer] = true;

                            pointer = (pointer + 1) % frameCount;
                            break;
                        }

                        referenceBits[pointer] = false;
                        pointer = (pointer + 1) % frameCount;
                    }
                }
            }

            result.Steps.Add(new SimulationStep {
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

}