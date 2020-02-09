using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene
{
    public class RightPanelComponents
    {
        public IInformatorPanel InformatorPanel { get; }
        public IMaskHighlightable SpeedButtonPanel { get; }
        public IMainMenuManager MainMenuManager { get; }

        public RightPanelComponents(
            IInformatorPanel informatorPanel, 
            IMaskHighlightable speedButtonPanel,
            IMainMenuManager mainMenuManager)
        {
            Helper.AssertIsNotNull(informatorPanel, speedButtonPanel, mainMenuManager);

            InformatorPanel = informatorPanel;
            SpeedButtonPanel = speedButtonPanel;
            MainMenuManager = mainMenuManager;
        }
    }
}