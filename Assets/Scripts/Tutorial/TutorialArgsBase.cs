using BattleCruisers.Cruisers;
using BattleCruisers.Data;
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
        public IApplicationModel AppModel { get; }
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

        public TutorialArgsBase(ITutorialArgsBase baseArgs)
            : this(
                baseArgs.AppModel,
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
            IApplicationModel appModel,
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
                appModel,
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

            AppModel = appModel;
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
