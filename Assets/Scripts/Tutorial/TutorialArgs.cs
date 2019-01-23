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
        public ICruiser PlayerCruiser { get; private set; }
        public ICruiser AICruiser { get; private set; }
        public ITutorialProvider TutorialProvider { get; private set; }
        public IPrefabFactory PrefabFactory { get; private set; }
        public IBattleSceneGodComponents Components { get; private set; }
        public ICameraComponents CameraComponents { get; private set; }
        public LeftPanelComponents LeftPanelComponents { get; private set; }
        public RightPanelComponents RightPanelComponents { get; private set; }
        public IUIManager UIManager { get; private set; }
        public IBattleCompletionHandler BattleCompletionHandler { get; private set; }

        public TutorialArgs(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            ITutorialProvider tutorialProvider,
            IPrefabFactory prefabFactory,
            IBattleSceneGodComponents battleSceneGodComponents,
            ICameraComponents cameraComponents,
            LeftPanelComponents leftPanelComponents,
            RightPanelComponents rightPanelComponents,
            IUIManager uiManager,
            IBattleCompletionHandler battleCompletionHandler)
        {
            Helper.AssertIsNotNull(
                playerCruiser, 
                aiCruiser, 
                tutorialProvider, 
                prefabFactory, 
                battleSceneGodComponents, 
                cameraComponents, 
                leftPanelComponents, 
                rightPanelComponents,
                uiManager,
                battleCompletionHandler);

            PlayerCruiser = playerCruiser;
            AICruiser = aiCruiser;
            TutorialProvider = tutorialProvider;
            PrefabFactory = prefabFactory;
            Components = battleSceneGodComponents;
            CameraComponents = cameraComponents;
            LeftPanelComponents = leftPanelComponents;
            RightPanelComponents = rightPanelComponents;
            UIManager = uiManager;
            BattleCompletionHandler = battleCompletionHandler;
        }
    }
}
