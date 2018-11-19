using System;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    public interface ICameraAdjuster
    {
        event EventHandler CompletedAdjustment;

        void AdjustCamera();
    }
}