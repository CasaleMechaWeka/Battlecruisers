using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils;

namespace BattleCruisers.Tutorial
{
    public class TutorialArgs : ITutorialArgs
    {
        public ICruiser PlayerCruiser { get; private set; }
        public ICruiser AICruiser { get; private set; }
        public INavigationButtonsWrapper NavigationButtonsWrapper { get; private set; }
        public IGameSpeedWrapper GameSpeedWrapper { get; private set; }
        public ICruiserInfo PlayerCruiserInfo { get; private set; }
        public IBuildMenuButtons BuildMenuButtons { get; private set; }
        public ITutorialProvider TutorialProvider { get; private set; }

        public TutorialArgs(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            IHUDCanvasController hudCanvas, 
            IBuildMenuButtons buildMenuButtons,
            ITutorialProvider tutorialProvider)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, hudCanvas, buildMenuButtons, tutorialProvider);

            PlayerCruiser = playerCruiser;
            AICruiser = aiCruiser;

            NavigationButtonsWrapper = hudCanvas.NavigationButtonsWrapper;
            GameSpeedWrapper = hudCanvas.GameSpeedWrapper;
            PlayerCruiserInfo = hudCanvas.PlayerCruiserInfo;

            BuildMenuButtons = buildMenuButtons;
            TutorialProvider = tutorialProvider;
        }
    }
}
