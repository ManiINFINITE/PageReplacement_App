namespace PRA.GUI.ViewModels;

public class ReferenceTokenViewModel(int value, bool isCurrent, bool isHit, bool isFault)
{
    public string Value => value.ToString();
    public bool IsCurrent => isCurrent;
    public bool IsHit => isHit;
    public bool IsFault => isFault;
}