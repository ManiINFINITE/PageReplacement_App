using PRA.Core.Algorithms;
using PRA.Core.Interfaces;

namespace PRA.CLI.Services;

public static class AlgorithmFactory
{
    public static List<IPageReplacementAlgorithm> GetAlgorithms()
    {
        return
        [
            new FifoAlgorithm(),
            new OptimalAlgorithm(),
            new ClockAlgorithm(),
            new LruAlgorithm(),
            new LfuAlgorithm()
        ];
    }
}