using BattleCruisers.Utils;
using UnityCommon.Properties;

namespace BattleCruisers.Projectiles.Spawners.Laser
{
    public interface ILaserEmitter : IManagedDisposable
    {
        IBroadcastingProperty<bool> IsLaserFiring { get; }

        void FireLaser(float angleInDegrees, bool isSourceMirrored);
        void StopLaser();
    }
}