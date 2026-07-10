namespace PRA.Core.Helpers;

public static class SimulationHelper {

    public static List<int?> CreateFrameSnapshot(List<int> frames, int frameCount) {
        var snapshot = new List<int?>(frameCount);

        for (int i = 0; i < frameCount; i++) {
            if (i < frames.Count)
                snapshot.Add(frames[i]);
            else
                snapshot.Add(null);
        }

        return snapshot;
    }

}