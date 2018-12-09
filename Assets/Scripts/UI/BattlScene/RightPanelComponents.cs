using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene
{
    public class RightPanelComponents
    {
        public IInformatorPanel InformatorPanel { get; private set; }
        public IMaskHighlightable SpeedButtonPanel { get; private set; }

        public RightPanelComponents(IInformatorPanel informatorPanel, IMaskHighlightable speedButtonPanel)
        {
            Helper.AssertIsNotNull(informatorPanel, speedButtonPanel);

            InformatorPanel = informatorPanel;
            SpeedButtonPanel = speedButtonPanel;
        }
    }
}