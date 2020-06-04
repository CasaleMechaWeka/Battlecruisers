using BattleCruisers.Utils;
using UnityCommon.Properties;

namespace BattleCruisers.Projectiles.Spawners.Beams.Laser
{
    public interface ILaserEmitter : IManagedDisposable
    {
        IBroadcastingProperty<bool> IsLaserFiring { get; }

        void FireBeam(float angleInDegrees, bool isSourceMirrored);
        void StopLaser();
    }
}