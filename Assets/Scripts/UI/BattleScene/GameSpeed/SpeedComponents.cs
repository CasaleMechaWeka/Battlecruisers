using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene.GameSpeed
{
    public class SpeedComponents
    {
        public IHighlightable SpeedButtonPanel { get; }
        public IToggleButtonGroup SpeedButtonGroup { get; }

        public SpeedComponents(IHighlightable speedButtonPanel, IToggleButtonGroup speedButtonGroup)
        {
            Helper.AssertIsNotNull(speedButtonPanel, speedButtonGroup);

            SpeedButtonPanel = speedButtonPanel;
            SpeedButtonGroup = speedButtonGroup;
        }
    }
}