using System;

namespace BattleCruisers.UI.Cameras
{
    public interface ICameraTargetProvider
    {
        ICameraTarget Target { get; }

        event EventHandler TargetChanged;
    }
}