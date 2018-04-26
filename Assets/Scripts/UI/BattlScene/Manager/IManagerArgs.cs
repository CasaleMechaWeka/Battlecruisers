using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Common.BuildableDetails;

namespace BattleCruisers.UI.BattleScene.Manager
{
    public interface IManagerArgs
    {
        ICruiser PlayerCruiser { get; }
        ICruiser AICruiser { get; }
        ICameraController CameraController { get; }
        IBuildMenu BuildMenu { get; }
        IBuildableDetailsManager DetailsManager { get; }
    }
}
