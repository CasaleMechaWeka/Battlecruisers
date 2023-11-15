using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public class PvPTopPanelComponents
    {
        public IPvPHighlightable PlayerLeftCruiserHealthBar { get; }
        public IPvPHighlightable PlayerRightCruiserHealthBar { get; }

        public PvPTopPanelComponents(IPvPHighlightable playerLeftCruiserHealthBar, IPvPHighlightable playerRightCruiserHealthBar)
        {
            PvPHelper.AssertIsNotNull(playerLeftCruiserHealthBar, playerRightCruiserHealthBar);

            PlayerLeftCruiserHealthBar = playerLeftCruiserHealthBar;
            PlayerRightCruiserHealthBar = playerRightCruiserHealthBar;
        }
    }
}