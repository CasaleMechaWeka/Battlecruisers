using BattleCruisers.Cruisers;
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
        
        public TutorialArgsNEW(
            ICruiser playerCruiser, 
            ICruiser aiCruiser, 
            ITutorialProvider tutorialProvider,
            IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, tutorialProvider, prefabFactory);

            PlayerCruiser = playerCruiser;
            AICruiser = aiCruiser;
            TutorialProvider = tutorialProvider;
            PrefabFactory = prefabFactory;
        }
    }
}
