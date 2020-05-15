using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene
{
    public class LeftPanelComponents
    {
        public IHighlightable NumberOfDronesHighlightable { get; }
        public IBuildMenu BuildMenu { get; }

        public LeftPanelComponents(IHighlightable numberOfDronesHighlightable, IBuildMenu buildMenu)
        {
            Helper.AssertIsNotNull(numberOfDronesHighlightable, buildMenu);

            NumberOfDronesHighlightable = numberOfDronesHighlightable;
            BuildMenu = buildMenu;
        }
    }
}