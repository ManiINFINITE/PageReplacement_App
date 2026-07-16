namespace PRA.GUI.ViewModels;

public class FrameViewModel(string label, string value, bool isHit, bool isFault, bool isReplaced)
{
    public string Label => label;
    public string Value => value;
    public bool IsHit => isHit;
    public bool IsFault => isFault;
    public bool IsReplaced => isReplaced;
}