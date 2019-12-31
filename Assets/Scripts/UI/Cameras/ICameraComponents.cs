using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
    public interface ICameraComponents
    {
        ICameraAdjuster CameraAdjuster { get; }
        INavigationWheel NavigationWheel { get; }
        ICameraFocuser CameraFocuser { get; }
        ICruiserDeathCameraFocuser CruiserDeathCameraFocuser { get; }
        Skybox Skybox { get; }
    }
}