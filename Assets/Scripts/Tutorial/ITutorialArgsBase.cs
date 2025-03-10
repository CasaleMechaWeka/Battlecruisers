using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Tutorial
{
    public interface ITutorialArgsBase
    {
        IApplicationModel AppModel { get; }
        ICruiser PlayerCruiser { get; }
        ICruiser AICruiser { get; }
        ITutorialProvider TutorialProvider { get; }
        IPrefabFactory PrefabFactory { get; }
        IBattleSceneGodComponents Components { get; }
        ICameraComponents CameraComponents { get; }
        TopPanelComponents TopPanelComponents { get; }
        LeftPanelComponents LeftPanelComponents { get; }
        RightPanelComponents RightPanelComponents { get; }
        IUIManager UIManager { get; }
        IGameEndMonitor GameEndMonitor { get; }
    }
}
