using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.Cameras;
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
        ICameraComponents CameraComponents { get; }
    }
}
