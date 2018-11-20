using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Tutorial
{
    public class TutorialArgsNEW : ITutorialArgsNEW
    {
        public ICruiser PlayerCruiser { get; private set; }
        public ICruiser AICruiser { get; private set; }
        public ITutorialProvider TutorialProvider { get; private set; }
        public IPrefabFactory PrefabFactory { get; private set; }
        public IBattleSceneGodComponents Components { get; private set; }
        public ICameraAdjuster CameraAdjuster { get; private set; }

        public TutorialArgsNEW(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            ITutorialProvider tutorialProvider,
            IPrefabFactory prefabFactory,
            IBattleSceneGodComponents battleSceneGodComponents,
            ICameraAdjuster cameraAdjuster)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, tutorialProvider, prefabFactory, battleSceneGodComponents, cameraAdjuster);

            PlayerCruiser = playerCruiser;
            AICruiser = aiCruiser;
            TutorialProvider = tutorialProvider;
            PrefabFactory = prefabFactory;
            Components = battleSceneGodComponents;
            CameraAdjuster = cameraAdjuster;
        }
    }
}
