using BattleCruisers.Projectiles.Spawners.Beams;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Laser
{
    public interface IPvPLaserEmitter : IBeamEmitter
    {
        IBroadcastingProperty<bool> IsLaserFiring { get; }

        void StopLaser();
    }
}