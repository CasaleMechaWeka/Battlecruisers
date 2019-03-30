using System;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public interface IScrollWheelCameraTargetProvider : ICameraTargetProvider
    {
        event EventHandler UserInputStarted;
        event EventHandler UserInputEnded;
    }
}