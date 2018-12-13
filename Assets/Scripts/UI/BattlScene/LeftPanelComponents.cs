using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene
{
    public class LeftPanelComponents
    {
        public IMaskHighlightable HealthDialHighlightable { get; private set; }
        public IMaskHighlightable NumberOfDronesHighlightable { get; private set; }
        public IBuildMenu BuildMenu { get; private set; }

        public LeftPanelComponents(
            IMaskHighlightable healthDialHighlightable, 
            IMaskHighlightable numberOfDronesHighlightable, 
            IBuildMenu buildMenu)
        {
            Helper.AssertIsNotNull(healthDialHighlightable, numberOfDronesHighlightable, buildMenu);

            HealthDialHighlightable = healthDialHighlightable;
            NumberOfDronesHighlightable = numberOfDronesHighlightable;
            BuildMenu = buildMenu;
        }
    }
}