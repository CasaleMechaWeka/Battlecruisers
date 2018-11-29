namespace BattleCruisers.Tutorial.Explanation
{
    public interface IExplanationPanel
    {
        ITextDisplayer TextDisplayer { get; }
        IExplanationDismissButton DismissButton { get; }
    }
}