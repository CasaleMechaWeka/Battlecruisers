using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Laser;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser
{
    public interface IPvPLaserCooldownEffectInitialiser
    {
        IPvPManagedDisposable CreateLaserCooldownEffect(IPvPLaserEmitter laserEmitter);
    }
}