using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Tutorial
{
    public class TutorialArgsNEW : ITutorialArgsNEW
    {
        public ICruiser PlayerCruiser { get; private set; }
        public ICruiser AICruiser { get; private set; }
        public ITutorialProvider TutorialProvider { get; private set; }
        public IPrefabFactory PrefabFactory { get; private set; }
        public ICamera Camera { get; private set; }

        public TutorialArgsNEW(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            ITutorialProvider tutorialProvider,
            IPrefabFactory prefabFactory,
            ICamera camera)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, tutorialProvider, prefabFactory, camera);

            PlayerCruiser = playerCruiser;
            AICruiser = aiCruiser;
            TutorialProvider = tutorialProvider;
            PrefabFactory = prefabFactory;
            Camera = camera;
        }
    }
}
