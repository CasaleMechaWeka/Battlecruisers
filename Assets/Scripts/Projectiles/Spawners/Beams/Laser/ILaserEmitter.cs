using UnityCommon.Properties;

namespace BattleCruisers.Projectiles.Spawners.Beams.Laser
{
    public interface ILaserEmitter : IBeamEmitter
    {
        IBroadcastingProperty<bool> IsLaserFiring { get; }

        void StopLaser();
    }
}