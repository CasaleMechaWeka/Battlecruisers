using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.Cameras.Adjusters;
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
        IBattleSceneGodComponents Components { get; }
        ICameraAdjuster CameraAdjuster { get; }
        // FELIX  Adapt for navigation wheel
		//INavigationSettings NavigationSettings { get; }
    }
}
