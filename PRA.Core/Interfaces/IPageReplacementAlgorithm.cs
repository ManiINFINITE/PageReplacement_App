using PRA.Core.Models;

namespace PRA.Core.Interfaces;

public interface IPageReplacementAlgorithm
{
    string Name { get; }

    SimulationResult Run(
        IReadOnlyList<int> referenceString,
        int frameCount
    );
}