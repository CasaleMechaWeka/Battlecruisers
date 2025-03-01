using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Explanation
{
    public interface IPvPExplanationPanel : IGameObject
    {
        IPvPTextDisplayer TextDisplayer { get; }
        IExplanationDismissButton OkButton { get; }
        IExplanationDismissButton DoneButton { get; }

        void ShrinkHeight();
        void ExpandHeight();
    }
}