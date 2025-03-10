using System;
using UnityEngine;

namespace BattleCruisers.Projectiles.Spawners.Beams.Laser
{
    public class LaserVisibilityChangedEventArgs : EventArgs
    {
        public bool IsLaserVisible { get; }

        public LaserVisibilityChangedEventArgs(bool isLaserVisible)
        {
            IsLaserVisible = isLaserVisible;
        }
    }

    public interface ILaserRenderer
    {
        event EventHandler<LaserVisibilityChangedEventArgs> LaserVisibilityChanged;

        void ShowLaser(Vector2 source, Vector2 target);
        void HideLaser();
    }
}
