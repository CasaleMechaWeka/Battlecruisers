using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Tutorial
{
    public class TutorialArgs : ITutorialArgs
    {
        public ICruiser PlayerCruiser { get; }
        public ICruiser AICruiser { get; }
        public ITutorialProvider TutorialProvider { get; }
        public IPrefabFactory PrefabFactory { get; }
        public IBattleSceneGodComponents Components { get; }
        public ICameraComponents CameraComponents { get; }
        public TopPanelComponents TopPanelComponents { get; }
        public LeftPanelComponents LeftPanelComponents { get; }
        public RightPanelComponents RightPanelComponents { get; }
        public IUIManager UIManager { get; }
        public IGameEndMonitor GameEndMonitor { get; }

        public TutorialArgs(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            ITutorialProvider tutorialProvider,
            IPrefabFactory prefabFactory,
            IBattleSceneGodComponents battleSceneGodComponents,
            ICameraComponents cameraComponents,
            TopPanelComponents topPanelComponents,
            LeftPanelComponents leftPanelComponents,
            RightPanelComponents rightPanelComponents,
            IUIManager uiManager,
            IGameEndMonitor gameEndMonitor)
        {
            Helper.AssertIsNotNull(
                playerCruiser, 
                aiCruiser, 
                tutorialProvider, 
                prefabFactory, 
                battleSceneGodComponents, 
                cameraComponents, 
                topPanelComponents,
                leftPanelComponents, 
                rightPanelComponents,
                uiManager,
                gameEndMonitor);

            PlayerCruiser = playerCruiser;
            AICruiser = aiCruiser;
            TutorialProvider = tutorialProvider;
            PrefabFactory = prefabFactory;
            Components = battleSceneGodComponents;
            CameraComponents = cameraComponents;
            TopPanelComponents = topPanelComponents;
            LeftPanelComponents = leftPanelComponents;
            RightPanelComponents = rightPanelComponents;
            UIManager = uiManager;
            GameEndMonitor = gameEndMonitor;
        }
    }
}
