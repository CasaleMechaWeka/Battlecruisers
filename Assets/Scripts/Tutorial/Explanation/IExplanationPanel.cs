using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Tutorial.Explanation
{
    public interface IExplanationPanel : IGameObject
    {
        ITextDisplayer TextDisplayer { get; }
        IExplanationDismissButton OkButton { get; }
        IExplanationDismissButton DoneButton { get; }

        void ShrinkHeight();
        void ExpandHeight();
    }
}