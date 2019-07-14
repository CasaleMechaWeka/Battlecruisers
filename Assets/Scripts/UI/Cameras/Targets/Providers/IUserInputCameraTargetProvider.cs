using System;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public interface IUserInputCameraTargetProvider : ICameraTargetProvider
    {
        event EventHandler UserInputStarted;
        event EventHandler UserInputEnded;
    }
}