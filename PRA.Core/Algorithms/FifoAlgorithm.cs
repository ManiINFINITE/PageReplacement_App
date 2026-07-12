using PRA.Core.Interfaces;
using PRA.Core.Models;
using PRA.Core.Utilities;

namespace PRA.Core.Algorithms;

public class FifoAlgorithm : IPageReplacementAlgorithm {

    public string Name => "FIFO";

    public SimulationResult Run(IReadOnlyList<int> referenceString, int frameCount) {
        var result = new SimulationResult {
            AlgorithmName = "FIFO"
        };

        if (frameCount <= 0 || referenceString.Count == 0)
            return result;

        var frames = new List<int>();
        var queue = new Queue<int>();

        foreach (var page in referenceString) {
            bool pageFault = false;
            int? replacedPage = null;

            if (frames.Contains(page)) {
                result.PageHits++;
            } else {
                pageFault = true;
                result.PageFaults++;

                if (frames.Count < frameCount) {
                    frames.Add(page);
                    queue.Enqueue(page);
                } else {
                    var victim = queue.Dequeue();
                    replacedPage = victim;

                    int victimIndex = frames.IndexOf(victim);
                    frames[victimIndex] = page;

                    queue.Enqueue(page);
                }
            }

            result.Steps.Add(new SimulationStep {
                CurrentPage = page,
                IsPageFault = pageFault,
                ReplacedPage = replacedPage,
                Frames = FrameSnapshot.CreateFrameSnapshot(frames, frameCount)
            });
        }

        return result;
    }

}