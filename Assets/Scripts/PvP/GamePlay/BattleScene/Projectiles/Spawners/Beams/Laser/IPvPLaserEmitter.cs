using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using BattleCruisers.Projectiles.Spawners.Beams;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Laser
{
    public interface IPvPLaserEmitter : IBeamEmitter
    {
        IPvPBroadcastingProperty<bool> IsLaserFiring { get; }

        void StopLaser();
    }
}