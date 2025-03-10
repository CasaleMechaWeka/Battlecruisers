using BattleCruisers.Projectiles.Spawners.Beams.Laser;
using BattleCruisers.Utils;

namespace BattleCruisers.Effects.Laser
{
    public interface ILaserCooldownEffectInitialiser
    {
        IManagedDisposable CreateLaserCooldownEffect(ILaserEmitter laserEmitter);
    }
}