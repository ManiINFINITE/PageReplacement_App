using PRA.Core.Algorithms;
using PRA.Core.Interfaces;
using System.Collections.Generic;

namespace PRA.GUI.Models;

public static class AlgorithmCatalog {

    public static IReadOnlyList<IPageReplacementAlgorithm> All { get; } = new List<IPageReplacementAlgorithm> {
        new FifoAlgorithm(),
        new OptimalAlgorithm(),
        new ClockAlgorithm(),
    };

}