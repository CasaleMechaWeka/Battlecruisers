using System;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public interface IUserInputCameraTargetProvider : ICameraTargetProvider
    {
        /// <summary>
        /// The higher the number the higher the priority.  This is so if we
        /// receive input from multiple camear target providers at the same time
        /// we can choose the provider with the higher priority.
        /// </summary>
        int Priority { get; }

        event EventHandler UserInputStarted;
        event EventHandler UserInputEnded;
    }
}