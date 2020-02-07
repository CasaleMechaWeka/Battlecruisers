using System;

namespace BattleCruisers.Effects
{
    public interface IBroadcastingAnimation : IAnimation
    {
        event EventHandler AnimationDone;
    }
}