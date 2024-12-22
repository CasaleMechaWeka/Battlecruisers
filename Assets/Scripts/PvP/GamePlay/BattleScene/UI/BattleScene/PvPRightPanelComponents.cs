using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.GameSpeed;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.MainMenu;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.HeckleMessage;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public class PvPRightPanelComponents
    {
        public IPvPInformatorPanel InformatorPanel { get; }
        public IPvPMainMenuManager MainMenuManager { get; }
        public IPvPModalMenu MainMenu { get; }
        // public PvPSpeedComponents SpeedComponents { get; }
        public PvPHelpButton HelpButton { get; }
        public PvPHecklePanelController HacklePanelController { get; }

        public PvPRightPanelComponents(
            IPvPInformatorPanel informatorPanel,
            IPvPMainMenuManager mainMenuManager,
            IPvPModalMenu mainMenu,
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