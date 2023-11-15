using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Explanation
{
    public interface IPvPExplanationPanel : IPvPGameObject
    {
        IPvPTextDisplayer TextDisplayer { get; }
        IPvPExplanationDismissButton OkButton { get; }
        IPvPExplanationDismissButton DoneButton { get; }

        void ShrinkHeight();
        void ExpandHeight();
    }
}