using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene
{
    public class RightPanelComponents
    {
        public IInformatorPanel InformatorPanel { get; }
        public IHighlightable SpeedButtonPanel { get; }
        public IMainMenuManager MainMenuManager { get; }

        public RightPanelComponents(
            IInformatorPanel informatorPanel, 
            IHighlightable speedButtonPanel,
            IMainMenuManager mainMenuManager)
        {
            Helper.AssertIsNotNull(informatorPanel, speedButtonPanel, mainMenuManager);

            InformatorPanel = informatorPanel;
            SpeedButtonPanel = speedButtonPanel;
            MainMenuManager = mainMenuManager;
        }
    }
}