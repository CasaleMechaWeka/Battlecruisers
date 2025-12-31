using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.HeckleMessage;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.MainMenu;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public class PvPRightPanelComponents
    {
        public PvPInformatorPanelController InformatorPanel { get; }
        public IMainMenuManager MainMenuManager { get; }
        public IModalMenu MainMenu { get; }
        // public PvPSpeedComponents SpeedComponents { get; }
        public PvPHelpButton HelpButton { get; }
        public PvPHecklePanelController HacklePanelController { get; }

        public PvPRightPanelComponents(
            PvPInformatorPanelController informatorPanel,
            IMainMenuManager mainMenuManager,
            IModalMenu mainMenu,
            //   PvPSpeedComponents speedComponents,
            PvPHecklePanelController pvPHecklePanelController,
            PvPHelpButton helpButton)
        {
            PvPHelper.AssertIsNotNull(informatorPanel, mainMenuManager, mainMenu, /*speedComponents,*/pvPHecklePanelController, helpButton);

            InformatorPanel = informatorPanel;
            MainMenuManager = mainMenuManager;
            MainMenu = mainMenu;
            //    SpeedComponents = speedComponents;
            HelpButton = helpButton;
            HacklePanelController = pvPHecklePanelController;
        }
    }
}