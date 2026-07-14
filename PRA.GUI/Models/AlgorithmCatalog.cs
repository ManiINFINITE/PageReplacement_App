using PRA.Core.Algorithms;
using PRA.Core.Interfaces;
using System.Collections.Generic;

namespace PRA.GUI.Models;

/// <summary>Single place listing every algorithm the GUI can run, so Compare
/// All / Compare Two / the New Simulation picker all stay in sync.</summary>
public static class AlgorithmCatalog {

    public static IReadOnlyList<IPageReplacementAlgorithm> All { get; } = new List<IPageReplacementAlgorithm> {
        new FifoAlgorithm(),
        new OptimalAlgorithm(),
        new ClockAlgorithm(),
    };

}