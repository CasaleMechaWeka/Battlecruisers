using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Tutorial.Highlighting;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public class PvPTopPanelComponents
    {
        public IPvPHighlightable PlayerCruiserHealthBar { get; }
        public IPvPHighlightable AICruiserHealthBar { get; }

        public PvPTopPanelComponents(IPvPHighlightable playerCruiserHealthBar, IPvPHighlightable aiCruiserHealthBar)
        {
            PvPHelper.AssertIsNotNull(playerCruiserHealthBar, aiCruiserHealthBar);

            PlayerCruiserHealthBar = playerCruiserHealthBar;
            AICruiserHealthBar = aiCruiserHealthBar;
        }
    }
}