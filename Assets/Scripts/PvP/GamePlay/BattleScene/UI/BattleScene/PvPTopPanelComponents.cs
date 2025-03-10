using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Tutorial.Highlighting;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public class PvPTopPanelComponents
    {
        public IHighlightable PlayerLeftCruiserHealthBar { get; }
        public IHighlightable PlayerRightCruiserHealthBar { get; }

        public PvPTopPanelComponents(IHighlightable playerLeftCruiserHealthBar, IHighlightable playerRightCruiserHealthBar)
        {
            PvPHelper.AssertIsNotNull(playerLeftCruiserHealthBar, playerRightCruiserHealthBar);

            PlayerLeftCruiserHealthBar = playerLeftCruiserHealthBar;
            PlayerRightCruiserHealthBar = playerRightCruiserHealthBar;
        }
    }
}