using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Tutorial.Explanation
{
    public interface IExplanationPanel : IGameObject
    {
        ITextDisplayer TextDisplayer { get; }
        IExplanationDismissButton OkButton { get; }
        IExplanationDismissButton DoneButton { get; }

        // FELIX  rename.  Append Height?
        void Shrink();
        void Expand();
    }
}