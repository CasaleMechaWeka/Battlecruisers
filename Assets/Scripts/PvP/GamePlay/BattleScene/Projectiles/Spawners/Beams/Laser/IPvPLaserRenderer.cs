using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Laser
{
    public class PvPLaserVisibilityChangedEventArgs : EventArgs
    {
        public bool IsLaserVisible { get; }

        public PvPLaserVisibilityChangedEventArgs(bool isLaserVisible)
        {
            IsLaserVisible = isLaserVisible;
        }
    }

    public interface IPvPLaserRenderer
    {
        event EventHandler<PvPLaserVisibilityChangedEventArgs> LaserVisibilityChanged;

        void ShowLaser(Vector2 source, Vector2 target);
        void HideLaser();
    }
}
