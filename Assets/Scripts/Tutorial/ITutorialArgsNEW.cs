using BattleCruisers.Cruisers;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Tutorial
{
    // FELIX  Expand as needed :)
    public interface ITutorialArgsNEW
    {
        ICruiser PlayerCruiser { get; }
        ICruiser AICruiser { get; }
        ITutorialProvider TutorialProvider { get; }
        IPrefabFactory PrefabFactory { get; }
        // FELIX  Adapt for navigation wheel
		//INavigationSettings NavigationSettings { get; }
    }
}
