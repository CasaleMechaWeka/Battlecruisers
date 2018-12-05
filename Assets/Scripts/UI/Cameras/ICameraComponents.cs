using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;

namespace BattleCruisers.UI.Cameras
{
    public interface ICameraComponents
    {
        ICameraAdjuster CameraAdjuster { get; }
        INavigationWheel NavigationWheel { get; }
        ICameraFocuser CameraFocuser { get; }
    }
}