using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils.BattleScene;

namespace BattleCruisers.Tutorial
{
    public interface ITutorialArgsBase
    {
        ICruiser PlayerCruiser { get; }
        ICruiser AICruiser { get; }
        ITutorialProvider TutorialProvider { get; }
        IBattleSceneGodComponents Components { get; }
        ICameraComponents CameraComponents { get; }
        TopPanelComponents TopPanelComponents { get; }
        LeftPanelComponents LeftPanelComponents { get; }
        RightPanelComponents RightPanelComponents { get; }
        IUIManager UIManager { get; }
        GameEndMonitor GameEndMonitor { get; }
    }
}
