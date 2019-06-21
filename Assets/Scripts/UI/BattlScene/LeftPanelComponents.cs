using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene
{
    public class LeftPanelComponents
    {
        public IMaskHighlightable NumberOfDronesHighlightable { get; }
        public IBuildMenu BuildMenu { get; }

        public LeftPanelComponents(IMaskHighlightable numberOfDronesHighlightable, IBuildMenu buildMenu)
        {
            Helper.AssertIsNotNull(numberOfDronesHighlightable, buildMenu);

            NumberOfDronesHighlightable = numberOfDronesHighlightable;
            BuildMenu = buildMenu;
        }
    }
}