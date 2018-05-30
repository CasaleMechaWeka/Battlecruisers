using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Tutorial
{
    public class TutorialArgs : ITutorialArgs
    {
        public ICruiser PlayerCruiser { get; private set; }
        public ICruiser AICruiser { get; private set; }
        public INavigationButtonsWrapper NavigationButtonsWrapper { get; private set; }
        public IGameSpeedWrapper GameSpeedWrapper { get; private set; }
        public ICruiserInfo PlayerCruiserInfo { get; private set; }
		public IBuildableDetails<IBuilding> BuildingDetails { get; private set; }
        public IBuildMenuButtons BuildMenuButtons { get; private set; }
        public ITutorialProvider TutorialProvider { get; private set; }
        public IPrefabFactory PrefabFactory { get; private set; }
		public INavigationSettings NavigationSettings { get; private set; }
		public ICameraMover CameraMover { get; private set; }
		public IUserInputCameraMover UserInputCameraMover { get; private set; }
        
        public TutorialArgs(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            IHUDCanvasController hudCanvas, 
            IBuildMenuButtons buildMenuButtons,
            ITutorialProvider tutorialProvider,
            IPrefabFactory prefabFactory,
			INavigationSettings navigationSettings,
			ICameraMover cameraMover,
			IUserInputCameraMover userInputCameraMover)
        {
			Helper.AssertIsNotNull(playerCruiser, aiCruiser, hudCanvas, buildMenuButtons, tutorialProvider, prefabFactory, navigationSettings, cameraMover, userInputCameraMover);

            PlayerCruiser = playerCruiser;
            AICruiser = aiCruiser;

            NavigationButtonsWrapper = hudCanvas.NavigationButtonsWrapper;
            GameSpeedWrapper = hudCanvas.GameSpeedWrapper;
            PlayerCruiserInfo = hudCanvas.PlayerCruiserInfo;
            BuildingDetails = hudCanvas.BuildingDetails;

            BuildMenuButtons = buildMenuButtons;
            TutorialProvider = tutorialProvider;
            PrefabFactory = prefabFactory;
			NavigationSettings = navigationSettings;
			CameraMover = cameraMover;
			UserInputCameraMover = userInputCameraMover;
        }
    }
}
