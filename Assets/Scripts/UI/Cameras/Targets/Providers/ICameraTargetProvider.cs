using System;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public interface ICameraTargetProvider
    {
        ICameraTarget Target { get; }

        event EventHandler TargetChanged;
    }
}