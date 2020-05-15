using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene
{
    public class TopPanelComponents
    {
        public IHighlightable PlayerCruiserHealthBar { get; }
        public IHighlightable AICruiserHealthBar { get; }

        public TopPanelComponents(IHighlightable playerCruiserHealthBar, IHighlightable aiCruiserHealthBar)
        {
            Helper.AssertIsNotNull(playerCruiserHealthBar, aiCruiserHealthBar);

            PlayerCruiserHealthBar = playerCruiserHealthBar;
            AICruiserHealthBar = aiCruiserHealthBar;
        }
    }
}