using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Tutorial.Explanation
{
    public interface IExplanationPanel : IGameObject
    {
        ITextDisplayer TextDisplayer { get; }
        IExplanationDismissButton DismissButton { get; }
    }
}