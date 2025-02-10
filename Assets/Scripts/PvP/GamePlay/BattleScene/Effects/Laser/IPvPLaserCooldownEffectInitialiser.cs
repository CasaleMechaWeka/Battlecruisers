using BattleCruisers.Projectiles.Spawners.Beams.Laser;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser
{
    public interface IPvPLaserCooldownEffectInitialiser
    {
        IManagedDisposable CreateLaserCooldownEffect(ILaserEmitter laserEmitter);
    }
}