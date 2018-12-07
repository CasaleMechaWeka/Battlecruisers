using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene
{
    public class LeftPanelComponents
    {
        public IMaskHighlightable HealthDialHighlightable { get; private set; }
        public IMaskHighlightable NumberOfDronesHighlightable { get; private set; }

        public LeftPanelComponents(IMaskHighlightable healthDialHighlightable, IMaskHighlightable numberOfDronesHighlightable)
        {
            Helper.AssertIsNotNull(healthDialHighlightable, numberOfDronesHighlightable);

            HealthDialHighlightable = healthDialHighlightable;
            NumberOfDronesHighlightable = numberOfDronesHighlightable;
        }
    }
}