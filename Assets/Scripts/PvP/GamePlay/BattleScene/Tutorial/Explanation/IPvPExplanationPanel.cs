using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Explanation
{
    public interface IPvPExplanationPanel : IGameObject
    {
        IPvPTextDisplayer TextDisplayer { get; }
        IPvPExplanationDismissButton OkButton { get; }
        IPvPExplanationDismissButton DoneButton { get; }

        void ShrinkHeight();
        void ExpandHeight();
    }
}