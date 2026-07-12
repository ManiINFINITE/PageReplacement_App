using PRA.Core.Interfaces;
using PRA.Core.Models;

namespace PRA.CLI.Services;

public class SimulationRunner {

    public SimulationResult Run(
        IPageReplacementAlgorithm algorithm,
        IReadOnlyList<int> referenceString,
        int frameCount
    ) {
        return algorithm.Run(referenceString, frameCount);
    }

}