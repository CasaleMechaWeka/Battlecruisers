using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene
{
    public class TopPanelComponents
    {
        public IMaskHighlightable PlayerCruiserHealthBar { get; }
        public IMaskHighlightable AICruiserHealthBar { get; }

        public TopPanelComponents(IMaskHighlightable playerCruiserHealthBar, IMaskHighlightable aiCruiserHealthBar)
        {
            Helper.AssertIsNotNull(playerCruiserHealthBar, aiCruiserHealthBar);

            PlayerCruiserHealthBar = playerCruiserHealthBar;
            AICruiserHealthBar = aiCruiserHealthBar;
        }
    }
}