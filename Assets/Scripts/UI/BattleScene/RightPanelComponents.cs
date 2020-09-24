using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.BattleScene
{
    public class RightPanelComponents
    {
        public IInformatorPanel InformatorPanel { get; }
        public IMainMenuManager MainMenuManager { get; }
        public SpeedComponents SpeedComponents { get; }

        public RightPanelComponents(
            IInformatorPanel informatorPanel, 
            IMainMenuManager mainMenuManager,
            SpeedComponents speedComponents)
        {
            Helper.AssertIsNotNull(informatorPanel, mainMenuManager, speedComponents);

            InformatorPanel = informatorPanel;
            MainMenuManager = mainMenuManager;
            SpeedComponents = speedComponents;
        }
    }
}