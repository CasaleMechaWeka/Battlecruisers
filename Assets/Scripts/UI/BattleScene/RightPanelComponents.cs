using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene
{
    public class RightPanelComponents
    {
        public IInformatorPanel InformatorPanel { get; }
        public IMainMenuManager MainMenuManager { get; }
        public IHighlightable MainMenuButton { get; }
        public SpeedComponents SpeedComponents { get; }

        public RightPanelComponents(
            IInformatorPanel informatorPanel, 
            IMainMenuManager mainMenuManager,
            IHighlightable mainMenuButton,
            SpeedComponents speedComponents)
        {
            Helper.AssertIsNotNull(informatorPanel, mainMenuManager, mainMenuButton, speedComponents);

            InformatorPanel = informatorPanel;
            MainMenuManager = mainMenuManager;
            MainMenuButton = mainMenuButton;
            SpeedComponents = speedComponents;
        }
    }
}