using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.MainMenu;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene
{
    public class RightPanelComponents
    {
        public IInformatorPanel InformatorPanel { get; }
        public IMainMenuManager MainMenuManager { get; }
        public IModalMenu MainMenu { get; }
        public SpeedComponents SpeedComponents { get; }
        public HelpButton HelpButton { get; }

        public RightPanelComponents(
            IInformatorPanel informatorPanel, 
            IMainMenuManager mainMenuManager,
            IModalMenu mainMenu,
            SpeedComponents speedComponents,
            HelpButton helpButton)
        {
            Helper.AssertIsNotNull(informatorPanel, mainMenuManager, mainMenu, speedComponents, helpButton);

            InformatorPanel = informatorPanel;
            MainMenuManager = mainMenuManager;
            MainMenu = mainMenu;
            SpeedComponents = speedComponents;
            HelpButton = helpButton;
        }
    }
}