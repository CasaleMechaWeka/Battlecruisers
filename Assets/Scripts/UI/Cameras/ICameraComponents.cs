using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
    public interface ICameraComponents
    {
        ICamera MainCamera { get; }
        ICameraAdjuster CameraAdjuster { get; }
        INavigationWheel NavigationWheel { get; }
        ICameraFocuser CameraFocuser { get; }
        ICruiserDeathCameraFocuser CruiserDeathCameraFocuser { get; }
        Skybox Skybox { get; }
        ICameraCalculatorSettings Settings { get; }
    }
}