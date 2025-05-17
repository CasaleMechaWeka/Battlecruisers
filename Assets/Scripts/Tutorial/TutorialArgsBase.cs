using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;

namespace BattleCruisers.Tutorial
{
    public class TutorialArgsBase : ITutorialArgsBase
    {
        public ICruiser PlayerCruiser { get; }
        public ICruiser AICruiser { get; }
        public ITutorialProvider TutorialProvider { get; }
        public BattleSceneGodComponents Components { get; }
        public CameraComponents CameraComponents { get; }
        public TopPanelComponents TopPanelComponents { get; }
        public LeftPanelComponents LeftPanelComponents { get; }
        public RightPanelComponents RightPanelComponents { get; }
        public UIManager UIManager { get; }
        public GameEndMonitor GameEndMonitor { get; }

        public TutorialArgsBase(ITutorialArgsBase baseArgs)
            : this(
                baseArgs.PlayerCruiser,
                baseArgs.AICruiser,
                baseArgs.TutorialProvider,
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
            BattleSceneGodComponents battleSceneGodComponents,
            CameraComponents cameraComponents,
            TopPanelComponents topPanelComponents,
            LeftPanelComponents leftPanelComponents,
            RightPanelComponents rightPanelComponents,
            UIManager uiManager,
            GameEndMonitor gameEndMonitor)
        {
            Helper.AssertIsNotNull(
                playerCruiser,
                aiCruiser,
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
