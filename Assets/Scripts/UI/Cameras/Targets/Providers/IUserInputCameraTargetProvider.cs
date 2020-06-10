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

        /// <summary>
        /// Once this event has fired the Target property (parent interface) is guaranteed
        /// to be up to date.  Before this event the Target property may be null (if this
        /// is the first time this event is fired), or stale (from the last user input
        /// session).
        /// </summary>
        event EventHandler UserInputStarted;
        event EventHandler UserInputEnded;
    }
}