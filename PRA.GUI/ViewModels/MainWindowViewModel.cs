using PRA.Core.Algorithms;
using System.Collections.Generic;

namespace PRA.GUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {

    public DashboardViewModel Dashboard { get; }

    public MainWindowViewModel() {
        // Placeholder input until the GUI has its own setup screen — swap for
        // whatever the user picks once you build that flow.
        var reference = new List<int> { 7, 0, 1, 2, 0, 3, 0, 4, 2, 3, 0, 3, 2 };
        var result = new FifoAlgorithm().Run(reference, 3);

        Dashboard = new DashboardViewModel(result, reference);
    }

}