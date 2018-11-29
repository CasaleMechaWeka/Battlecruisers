namespace BattleCruisers.Tutorial.Explanation
{
    public interface IExplanationPanel
    {
        ITextDisplayer TextDisplayer { get; }
        IClickableEmitter DismissButton { get; }
    }
}