using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Tutorial
{
    public interface ITutorialArgs
    {
        ICruiser PlayerCruiser { get; }
        ICruiser AICruiser { get; }
        ITutorialProvider TutorialProvider { get; }
        IPrefabFactory PrefabFactory { get; }
        IBattleSceneGodComponents Components { get; }
        ICameraComponents CameraComponents { get; }
        LeftPanelComponents LeftPanelComponents { get; }
        RightPanelComponents RightPanelComponents { get; }
        IUIManager UIManager { get; }
    }
}
