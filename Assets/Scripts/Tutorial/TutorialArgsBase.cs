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
    public class TutorialArgsBase : ITutorialArgsBase
    {
        public ICruiser PlayerCruiser { get; }
        public ICruiser AICruiser { get; }
        public ITutorialProvider TutorialProvider { get; }
        public PrefabFactory PrefabFactory { get; }
        public IBattleSceneGodComponents Components { get; }
        public ICameraComponents CameraComponents { get; }
        public TopPanelComponents TopPanelComponents { get; }
        public LeftPanelComponents LeftPanelComponents { get; }
        public RightPanelComponents RightPanelComponents { get; }
        public IUIManager UIManager { get; }
        public GameEndMonitor GameEndMonitor { get; }

        public TutorialArgsBase(ITutorialArgsBase baseArgs)
            : this(
                baseArgs.PlayerCruiser,
                baseArgs.AICruiser,
                baseArgs.TutorialProvider,
                baseArgs.PrefabFactory,
                baseArgs.Components,
                baseArgs.CameraComponents,
                baseArgs.TopPanelComponents,
                baseArgs.LeftPanelComponents,
                baseArgs.RightPanelComponents,
                baseArgs.UIManager,
                baseArgs.GameEndMonitor)
        { }

        public TutorialArgsBase(
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            ITutorialProvider tutorialProvider,
            PrefabFactory prefabFactory,
            IBattleSceneGodComponents battleSceneGodComponents,
            ICameraComponents cameraComponents,
            TopPanelComponents topPanelComponents,
            LeftPanelComponents leftPanelComponents,
            RightPanelComponents rightPanelComponents,
            IUIManager uiManager,
            GameEndMonitor gameEndMonitor)
        {
            Helper.AssertIsNotNull(
                playerCruiser,
                aiCruiser,
                prefabFactory,
                battleSceneGodComponents,
                cameraComponents,
                topPanelComponents,
                leftPanelComponents,
                rightPanelComponents,
                uiManager,
                gameEndMonitor);
            // tutorialProvider may be null :/

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
